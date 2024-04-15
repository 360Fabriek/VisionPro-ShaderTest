using System.Collections;
using UnityEngine;

namespace PolySpatial.Template
{
    /// <summary>
    /// A visual guide that is used in the placement of objects on planes.
    /// </summary>
    public class VerticalGuide : MonoBehaviour
    {
        //The pole/string section of the guide
        [SerializeField]
        Transform m_Guide;

        //The circular section of the guide
        [SerializeField]
        Transform m_Plane;

        void OnEnable()
        {
            StartCoroutine(Animate(m_Plane.GetComponent<MeshRenderer>().material, 0f, 1f, 0.5f));
        }

        /// <summary>
        /// Sets the size of the pole/string to the given parameter "height"
        /// </summary>
        public void SetGuideHeight(float height)
        {
            if (m_Guide != null)
            {
                var localScale = m_Guide.localScale;
                m_Guide.localScale = new Vector3(localScale.x, height / 2f, localScale.z);
            }
        }

        /// <summary>
        /// Animates the circular section of the guide.
        /// </summary>
        static IEnumerator Animate(Material material, float start, float end, float duration)
        {
            var increment = 0f;
            while (increment <= duration)
            {
                increment += Time.deltaTime;
                var percent = Mathf.Clamp01(increment / duration);
                var newAlpha = Mathf.Lerp(start, end, percent);
                var tempColor = material.color;
                tempColor.a = newAlpha;
                material.color = tempColor;
                yield return null;
            }
        }
    }
}
