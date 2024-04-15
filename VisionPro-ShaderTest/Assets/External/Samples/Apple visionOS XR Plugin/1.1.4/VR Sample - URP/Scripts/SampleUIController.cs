using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

#if INCLUDE_UNITY_XR_HANDS && (UNITY_VISIONOS || UNITY_EDITOR)
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
#endif

namespace UnityEngine.XR.VisionOS.Samples.URP
{
    public class SampleUIController : MonoBehaviour
    {
#if INCLUDE_UNITY_XR_HANDS && (UNITY_VISIONOS || UNITY_EDITOR)
        const string k_CreateHandSubsystemText = "Create Hand Subsystem";
        const string k_DestroyHandSubsystemText = "Destroy Hand Subsystem";
#endif

        [SerializeField]
        ParticleSystem m_ParticleSystem;

        [SerializeField]
        ARAnchorManager m_AnchorManager;

        [SerializeField]
        GameObject m_HandSubsystemToggleButton;

        [SerializeField]
        Text m_HandSubsystemToggleText;

#if INCLUDE_UNITY_XR_HANDS && (UNITY_VISIONOS || UNITY_EDITOR)
        VisionOSLoader m_Loader;
        XRHandSubsystem m_HandSubsystem;
#endif

        static readonly List<ARAnchor> k_AnchorsToDestroy = new();

        void Awake()
        {
#if INCLUDE_UNITY_XR_HANDS && (UNITY_VISIONOS || UNITY_EDITOR)
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
                m_Loader = XRGeneralSettings.Instance.Manager.ActiveLoaderAs<VisionOSLoader>();

            // If building in Windowed or Mixed Reality mode, VisionOSLoader may not be active
            if (m_Loader == null)
            {
                m_HandSubsystemToggleButton.SetActive(false);
                return;
            }

            m_HandSubsystem = m_Loader.handSubsystem;
            UpdateHandSubsystemToggleText();
#else
            m_HandSubsystemToggleButton.SetActive(false);
#endif
        }

        public void SetParticleStartSpeed(float speed)
        {
            var mainModule = m_ParticleSystem.main;
            mainModule.simulationSpeed = speed;
        }

        public void ClearWorldAnchors()
        {
            if (m_AnchorManager == null)
            {
                Debug.LogError("Cannot clear world anchors; Anchor Manager is null");
                return;
            }

            var anchorSubsystem = m_AnchorManager.subsystem;
            if (anchorSubsystem == null || !anchorSubsystem.running)
            {
                Debug.LogWarning("Cannot clear anchors if subsystem is not running");
                return;
            }

            // Copy anchors to a reusable list to avoid InvalidOperationException caused by Destroy modifying the list of anchors
            k_AnchorsToDestroy.Clear();
            foreach (var anchor in m_AnchorManager.trackables)
            {
                k_AnchorsToDestroy.Add(anchor);
            }

            foreach (var anchor in k_AnchorsToDestroy)
            {
                Debug.Log($"Destroying anchor with trackable id: {anchor.trackableId.ToString()}");
                Destroy(anchor.gameObject);
            }

            k_AnchorsToDestroy.Clear();
        }

        public void ToggleHandSubsystem()
        {
#if INCLUDE_UNITY_XR_HANDS && (UNITY_VISIONOS || UNITY_EDITOR)
            if (m_Loader == null)
                return;

            if (m_HandSubsystem == null)
            {
                m_Loader.CreateHandSubsystem();
                m_Loader.StartHandSubsystem();
                m_HandSubsystem = m_Loader.handSubsystem;
            }
            else
            {
                m_Loader.DestroyHandSubsystem();
                m_HandSubsystem = null;
            }

            UpdateHandSubsystemToggleText();
#endif
        }

#if INCLUDE_UNITY_XR_HANDS && (UNITY_VISIONOS || UNITY_EDITOR)
        void UpdateHandSubsystemToggleText()
        {
            m_HandSubsystemToggleText.text = m_HandSubsystem == null ? k_CreateHandSubsystemText : k_DestroyHandSubsystemText;
        }
#endif
    }
}
