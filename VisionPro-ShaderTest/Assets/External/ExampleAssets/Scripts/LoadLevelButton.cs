using UnityEngine;

namespace PolySpatial.Template
{
    public class LoadLevelButton : MonoBehaviour
    {
        [SerializeField]
        string m_LevelName;

        public void LoadLevel()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(m_LevelName);
        }
    }
}
