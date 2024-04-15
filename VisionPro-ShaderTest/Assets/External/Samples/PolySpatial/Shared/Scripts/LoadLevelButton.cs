using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace PolySpatial.Samples
{
    public class LoadLevelButton : HubButton
    {
        [SerializeField]
        string m_LevelName;

        [SerializeField]
        LevelData.LevelTypes m_LevelType;

        [SerializeField]
        ARSession m_ARSession;

        public LevelData.LevelTypes LevelType => m_LevelType;

        public override void Press()
        {
            base.Press();

            if (m_ARSession != null)
            {
                // Disable the session before closing the immersive space to avoid a crash
                m_ARSession.enabled = false;
            }

            SceneManager.LoadScene(m_LevelName);
        }
    }
}
