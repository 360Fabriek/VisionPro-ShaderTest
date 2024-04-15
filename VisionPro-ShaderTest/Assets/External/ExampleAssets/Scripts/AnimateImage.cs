using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PolySpatial.Template
{
    /// <summary>
    /// On awake, animates image from start size to max size, and/or fades image from full alpha to no alpha.
    /// </summary>
    public class AnimateImage : MonoBehaviour
    {
        [SerializeField]
        Image m_Image;

        [SerializeField]
        bool m_AnimateSize;

        [SerializeField]
        float m_StartSize = 15f;

        [SerializeField]
        float m_MaxSize = 100f;

        [SerializeField]
        AnimationCurve m_AnimationCurve;

        [SerializeField]
        bool m_AnimateOpacity = true;

        [SerializeField]
        AnimationCurve m_FadeCurve;

        [SerializeField]
        float m_Duration = 0.5f;

        IEnumerator Start()
        {
            if (m_AnimateSize)
                m_Image.rectTransform.sizeDelta = Vector2.zero;

            var increment = 0f;
            while (increment <= m_Duration)
            {
                increment += Time.deltaTime;
                var percent = Mathf.Clamp01(increment / m_Duration);
                if (m_AnimateSize)
                {
                    var curvePercent = m_AnimationCurve.Evaluate(percent);
                    var newSize = Mathf.Lerp(m_StartSize, m_MaxSize, curvePercent);
                    m_Image.rectTransform.sizeDelta = new Vector2(newSize, newSize);
                }

                if (m_AnimateOpacity)
                {
                    var fadePercent = m_FadeCurve.Evaluate(percent);
                    var tempColor = m_Image.color;
                    tempColor.a = 1f - fadePercent;
                    m_Image.color = tempColor;
                }

                yield return null;
            }
        }
    }
}
