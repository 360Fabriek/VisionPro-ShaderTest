using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PolySpatial.Template
{
    public class SpatialUIDropDown : SpatialUI
    {
        [SerializeField]
        GameObject m_ExpandedContent;

        [SerializeField]
        GameObject m_OpenIcon;

        [SerializeField]
        GameObject m_ClosedIcon;

        [SerializeField]
        TMP_Text m_CurrentSelectionText;

        [SerializeField]
        List<SpatialUIButton> m_ContentButtons;

        [SerializeField]
        UnityEvent<int> m_DropDownValueChanged;

        [SerializeField]
        int m_CurrentAssetIndex;

        bool m_IsShowingExpandedContent;

        public int CurrentAssetIndex => m_CurrentAssetIndex;
        public UnityEvent<int> DropDownValueChangedEvent => m_DropDownValueChanged;

        void OnEnable()
        {
            foreach (var button in m_ContentButtons)
            {
                button.WasPressed += WasPressed;
            }
        }

        void OnDisable()
        {
            foreach (var button in m_ContentButtons)
            {
                button.WasPressed -= WasPressed;
            }

            m_ExpandedContent.SetActive(false);
            m_OpenIcon.SetActive(false);
            m_ClosedIcon.SetActive(true);
        }

        void WasPressed(string text, MeshRenderer meshRenderer, int currentIndex)
        {
            m_CurrentSelectionText.text = text;
            m_IsShowingExpandedContent = false;
            meshRenderer.material.color = m_SelectedColor;
            m_ExpandedContent.SetActive(false);
            m_OpenIcon.SetActive(false);
            m_ClosedIcon.SetActive(true);
            m_CurrentAssetIndex = currentIndex;
            m_DropDownValueChanged.Invoke(currentIndex);
        }

        public override void PressEnd()
        {
            base.PressEnd();
            m_IsShowingExpandedContent = !m_IsShowingExpandedContent;

            m_OpenIcon.SetActive(m_IsShowingExpandedContent);
            m_ClosedIcon.SetActive(!m_IsShowingExpandedContent);
            m_ExpandedContent.SetActive(m_IsShowingExpandedContent);

            foreach (var button in m_ContentButtons)
            {
                if (m_CurrentSelectionText.text != button.ButtonText)
                {
                    button.MeshRenderer.material.color = m_UnselectedColor;
                }
            }
        }
    }
}
