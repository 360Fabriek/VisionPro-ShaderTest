using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using LazyFollow = UnityEngine.XR.Interaction.Toolkit.UI.LazyFollow;

namespace PolySpatial.Template
{
    public class GoalManager : MonoBehaviour
    {
        enum OnboardingGoals
        {
            Empty,
            FindSurfaces,
            TapSurface,
        }

        struct Goal
        {
            public OnboardingGoals CurrentGoal;
            public bool Completed;

            public Goal(OnboardingGoals goal)
            {
                CurrentGoal = goal;
                Completed = false;
            }
        }

        [Serializable]
        struct Step
        {
            public GameObject StepObject;
            public string ButtonText;
            public bool IncludeSkipButton;
        }

        Queue<Goal> m_OnboardingGoals;
        Goal m_CurrentGoal;
        bool m_AllGoalsFinished;
        int m_SurfacesTapped;
        int m_CurrentGoalIndex;

        [SerializeField]
        List<Step> m_StepList = new();

        [SerializeField]
        TMP_Text m_StepButtonTextField;

        [SerializeField]
        GameObject m_SkipButton;

        [SerializeField]
        GameObject m_LearnButton;

        [SerializeField]
        GameObject m_LearnModal;

        [SerializeField]
        Button m_LearnModalButton;

        [SerializeField]
        GameObject m_CoachingUIParent;

        [SerializeField]
        Toggle m_PassThroughToggle;

        [SerializeField]
        LazyFollow m_GoalPanelLazyFollow;

        [SerializeField]
        GameObject m_TapTooltip;

        [SerializeField]
        Toggle m_VideoPlayerToggle;

        [SerializeField]
        ARPlaneManager m_ARPlaneManager;

        [SerializeField]
        ObjectSpawner m_ObjectSpawner;

        const int k_NumberOfSurfacesTappedToCompleteGoal = 1;

        void Start()
        {
            m_OnboardingGoals = new Queue<Goal>();
            var welcomeGoal = new Goal(OnboardingGoals.Empty);
            var findSurfaceGoal = new Goal(OnboardingGoals.FindSurfaces);
            var tapSurfaceGoal = new Goal(OnboardingGoals.TapSurface);
            var endGoal = new Goal(OnboardingGoals.Empty);

            m_OnboardingGoals.Enqueue(welcomeGoal);
            m_OnboardingGoals.Enqueue(findSurfaceGoal);
            m_OnboardingGoals.Enqueue(tapSurfaceGoal);
            m_OnboardingGoals.Enqueue(endGoal);

            m_CurrentGoal = m_OnboardingGoals.Dequeue();
            if (m_TapTooltip != null)
                m_TapTooltip.SetActive(false);

            if (m_LearnButton != null)
            {
                m_LearnButton.GetComponent<Button>().onClick.AddListener(OpenModal);
                m_LearnButton.SetActive(false);
            }

            if (m_LearnModal != null)
            {
                m_LearnModal.transform.localScale = Vector3.zero;
            }

            if (m_LearnModalButton != null)
            {
                m_LearnModalButton.onClick.AddListener(CloseModal);
            }
        }

        void OpenModal()
        {
            if (m_LearnModal != null)
            {
                m_LearnModal.transform.localScale = Vector3.one;
            }
        }

        void CloseModal()
        {
            if (m_LearnModal != null)
            {
                m_LearnModal.transform.localScale = Vector3.zero;
            }
        }

        void Update()
        {
            if (!m_AllGoalsFinished)
            {
                ProcessGoals();
            }

            // Debug Input
#if UNITY_EDITOR
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                CompleteGoal();
            }
#endif
        }

        void ProcessGoals()
        {
            if (!m_CurrentGoal.Completed)
            {
                switch (m_CurrentGoal.CurrentGoal)
                {
                    case OnboardingGoals.Empty:
                        m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
                        break;
                    case OnboardingGoals.FindSurfaces:
                        m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
                        break;
                    case OnboardingGoals.TapSurface:
                        if (m_TapTooltip != null)
                        {
                            m_TapTooltip.SetActive(true);
                        }

                        m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.None;
                        break;
                }
            }
        }

        void CompleteGoal()
        {

            if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface)
                m_ObjectSpawner.objectSpawned -= OnObjectSpawned;

            // disable tooltips before setting next goal
            DisableTooltips();

            m_CurrentGoal.Completed = true;
            m_CurrentGoalIndex++;
            if (m_OnboardingGoals.Count > 0)
            {
                m_CurrentGoal = m_OnboardingGoals.Dequeue();
                m_StepList[m_CurrentGoalIndex - 1].StepObject.SetActive(false);
                m_StepList[m_CurrentGoalIndex].StepObject.SetActive(true);
                m_StepButtonTextField.text = m_StepList[m_CurrentGoalIndex].ButtonText;
                m_SkipButton.SetActive(m_StepList[m_CurrentGoalIndex].IncludeSkipButton);
            }
            else
            {
                m_AllGoalsFinished = true;
                ForceEndAllGoals();
            }

            switch (m_CurrentGoal.CurrentGoal)
            {
                case OnboardingGoals.FindSurfaces:
                {
                    if (m_PassThroughToggle != null)
                        m_PassThroughToggle.isOn = true;

                    if (m_LearnButton != null)
                    {
                        m_LearnButton.SetActive(true);
                    }

                    StartCoroutine(TurnOnPlanes());
                    break;
                }
                case OnboardingGoals.TapSurface:
                {
                    if (m_LearnButton != null)
                    {
                        m_LearnButton.SetActive(false);
                    }

                    m_SurfacesTapped = 0;
                    m_ObjectSpawner.objectSpawned += OnObjectSpawned;
                    break;
                }
            }
        }

        IEnumerator TurnOnPlanes()
        {
            yield return new WaitForSeconds(1f);
            m_ARPlaneManager.enabled = true;
        }

        void DisableTooltips()
        {
            if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface)
            {
                if (m_TapTooltip != null)
                {
                    m_TapTooltip.SetActive(false);
                }
            }
        }

        public void ForceCompleteGoal()
        {
            CompleteGoal();
        }

        public void ForceEndAllGoals()
        {
            m_CoachingUIParent.transform.localScale = Vector3.zero;

            if (m_VideoPlayerToggle != null)
                m_VideoPlayerToggle.isOn = true;

            if (m_LearnButton != null)
            {
                m_LearnButton.SetActive(false);
            }

            if (m_LearnModal != null)
            {
                m_LearnModal.transform.localScale = Vector3.zero;
            }

            StartCoroutine(TurnOnPlanes());
        }

        public void ResetCoaching()
        {
            m_CoachingUIParent.transform.localScale = Vector3.one;

            m_OnboardingGoals.Clear();
            m_OnboardingGoals = new Queue<Goal>();
            var welcomeGoal = new Goal(OnboardingGoals.Empty);
            var findSurfaceGoal = new Goal(OnboardingGoals.FindSurfaces);
            var tapSurfaceGoal = new Goal(OnboardingGoals.TapSurface);
            var endGoal = new Goal(OnboardingGoals.Empty);

            m_OnboardingGoals.Enqueue(welcomeGoal);
            m_OnboardingGoals.Enqueue(findSurfaceGoal);
            m_OnboardingGoals.Enqueue(tapSurfaceGoal);
            m_OnboardingGoals.Enqueue(endGoal);

            for (var i = 0; i < m_StepList.Count; i++)
            {
                if (i == 0)
                {
                    m_StepList[i].StepObject.SetActive(true);
                    m_SkipButton.SetActive(m_StepList[i].IncludeSkipButton);
                    m_StepButtonTextField.text = m_StepList[i].ButtonText;
                }
                else
                {
                    m_StepList[i].StepObject.SetActive(false);
                }
            }

            m_CurrentGoal = m_OnboardingGoals.Dequeue();
            m_AllGoalsFinished = false;

            if (m_TapTooltip != null)
                m_TapTooltip.SetActive(false);

            if (m_LearnButton != null)
            {
                m_LearnButton.SetActive(false);
            }

            if (m_LearnModal != null)
            {
                m_LearnModal.transform.localScale = Vector3.zero;
            }

            m_CurrentGoalIndex = 0;
        }

        void OnObjectSpawned(GameObject spawnedObject)
        {
            m_SurfacesTapped++;
            if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface && m_SurfacesTapped >= k_NumberOfSurfacesTappedToCompleteGoal)
            {
                CompleteGoal();
                m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
            }
        }
    }
}
