using System;
using UnityEngine;

namespace PolySpatial.Template
{
    public class SpatialUIButton : SpatialUI
    {
        [SerializeField]
        string m_ButtonText;

        [SerializeField]
        int m_ButtonIndex;

        MeshRenderer m_MeshRenderer;

        public string ButtonText => m_ButtonText;
        public MeshRenderer MeshRenderer => m_MeshRenderer;
        public event Action<string, MeshRenderer, int> WasPressed;

        void OnEnable()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
        }

        public override void PressStart()
        {
            m_PressStart.Invoke();
            base.PressStart();
        }

        public override void PressEnd()
        {
            m_PressEnd.Invoke();
            base.PressEnd();
            WasPressed?.Invoke(m_ButtonText, m_MeshRenderer, m_ButtonIndex);
        }
    }
}
