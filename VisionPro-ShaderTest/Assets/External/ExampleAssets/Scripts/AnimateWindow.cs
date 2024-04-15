using System.Collections;
using UnityEngine;

namespace PolySpatial.Template
{
    /// <summary>
    /// On enable, animates the position of given rect from start to end positions, as well as animating scale from 0 to original scale.
    /// </summary>
    public class AnimateWindow : MonoBehaviour
    {
        [SerializeField]
        Vector3 m_StartPosition;

        [SerializeField]
        Vector3 m_EndPosition;

        [SerializeField]
        RectTransform m_Rect;

        [SerializeField]
        AnimationCurve m_AnimationCurve;

        [SerializeField]
        float m_Duration = 0.5f;

        Vector3 m_StartingScale;

        void Awake()
        {
            m_StartingScale = m_Rect.localScale;
        }

        void OnEnable()
        {
            m_Rect.localScale = Vector3.zero;
            StartCoroutine(Animate(m_Duration));
        }

        IEnumerator Animate(float duration)
        {
            var increment = 0f;
            while (increment <= duration)
            {
                increment += Time.deltaTime;
                var percent = Mathf.Clamp01(increment / duration);
                var curvePercent = m_AnimationCurve.Evaluate(percent);
                var newPosition = Vector3.Lerp(m_StartPosition, m_EndPosition, curvePercent);
                m_Rect.localPosition = newPosition;
                m_Rect.localScale = Vector3.one * (curvePercent * m_StartingScale.x);
                yield return null;
            }
        }
    }
}
