using System;
using UnityEngine;

namespace PolySpatial.Samples
{
    class ShowInSimulatorBehavior : MonoBehaviour
    {
        [SerializeField]
        GameObject m_ObjectToShow;

        [SerializeField]
        GameObject m_ObjectToHide;

        void Start()
        {
            var simRoot = Environment.GetEnvironmentVariable("SIMULATOR_ROOT") != null;
            m_ObjectToShow.SetActive(simRoot);
            m_ObjectToHide.SetActive(!simRoot);
        }
    }
}
