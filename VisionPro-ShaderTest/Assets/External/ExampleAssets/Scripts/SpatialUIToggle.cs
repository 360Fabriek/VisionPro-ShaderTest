using UnityEngine;
using UnityEngine.Events;

namespace PolySpatial.Template
{
    public class SpatialUIToggle : SpatialUI
    {
        [SerializeField]
        UnityEvent<bool> m_ToggleChanged;

        [SerializeField]
        MeshRenderer m_ToggleBackground;

        bool m_Active;

        public override void PressStart()
        {
            base.PressStart();
            m_PressStart.Invoke();
        }

        public override void PressEnd()
        {
            m_PressEnd.Invoke();
            base.PressEnd();
            m_Active = !m_Active;
            m_ToggleChanged.Invoke(m_Active);
            m_ToggleBackground.material.color = m_Active ? m_SelectedColor : m_UnselectedColor;
        }
    }
}
