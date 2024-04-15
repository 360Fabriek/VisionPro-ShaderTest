using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace PolySpatial.Template
{
    public class GazeTooltips : MonoBehaviour
    {
        [SerializeField]
        XROrigin m_XROrigin;

        [SerializeField]
        Transform m_TooltipTransform;

        [SerializeField]
        bool m_TapTooltip = true;

        Transform m_XRCameraTransform;
        LayerMask m_PlaneMask;
        readonly List<ARContactSpawnTrigger> k_ContactTriggers = new();

        const float k_SphereCastRadius = 0.1f;
        const string k_PlaneLayer = "Placeable Surface";

        void Awake()
        {
            if (m_XROrigin == null)
                m_XROrigin = FindAnyObjectByType<XROrigin>();

            m_XRCameraTransform = m_XROrigin.Camera.transform;
            m_XROrigin.GetComponentsInChildren(true, k_ContactTriggers);
            m_PlaneMask = LayerMask.GetMask(k_PlaneLayer);
        }

        void LateUpdate()
        {
            PlaceTooltip();
        }

        void PlaceTooltip()
        {
            if (Physics.SphereCast(new Ray(m_XRCameraTransform.position, m_XRCameraTransform.forward), k_SphereCastRadius, out var hitInfo, float.MaxValue,
                    m_PlaneMask))
            {
                var spawnSurfaceFound = false;
                var surfacePosition = Vector3.zero;

                // plane tap tooltip
                if (m_TapTooltip)
                {
                    foreach (var contactTrigger in k_ContactTriggers)
                    {
                        if (contactTrigger.isActiveAndEnabled && contactTrigger.TryGetSpawnSurfaceData(hitInfo.collider, out surfacePosition, out _))
                        {
                            spawnSurfaceFound = true;
                            break;
                        }
                    }
                }

                if (!spawnSurfaceFound)
                    return;

                m_TooltipTransform.position = surfacePosition;
                var facePosition = m_XRCameraTransform.position;
                var forward = facePosition - m_TooltipTransform.position;
                var targetRotation = forward.sqrMagnitude > float.Epsilon ? Quaternion.LookRotation(forward, Vector3.up) : Quaternion.identity;
                targetRotation *= Quaternion.Euler(new Vector3(0f, 180f, 0f));
                var targetEuler = targetRotation.eulerAngles;
                var currentEuler = m_TooltipTransform.rotation.eulerAngles;
                targetRotation = Quaternion.Euler
                (
                    currentEuler.x,
                    targetEuler.y,
                    currentEuler.z
                );

                m_TooltipTransform.rotation = targetRotation;

                if (!m_TooltipTransform.gameObject.activeSelf)
                    m_TooltipTransform.gameObject.SetActive(true);
            }
        }
    }
}
