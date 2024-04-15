using System.Collections;
using UnityEngine;

namespace PolySpatial.Template
{
    /// <summary>
    /// Does fade/lerp style animations on the properties of material "PlaneMaterial".
    /// </summary>
    public class FadePlaneMaterial : MonoBehaviour
    {
        [SerializeField]
        Renderer m_Renderer;

        [SerializeField]
        float m_DotFadeDuration = 1f;

        [SerializeField]
        float m_AlphaFadeDuration = 0.5f;

        [SerializeField]
        float m_MaximumDotViewRadius = 1.5f;

        [SerializeField]
        float m_MaximumAlpha = 0.5f;

        Coroutine m_FadeDotsCoroutine;
        Coroutine m_FadeAlphaCoroutine;
        static readonly int k_DotViewRadius = Shader.PropertyToID("_DotViewRadius");
        static readonly int k_Alpha = Shader.PropertyToID("_Alpha");

        void Awake()
        {
            m_Renderer.material.SetFloat(k_DotViewRadius, 0f);
            m_Renderer.material.SetFloat(k_Alpha, 0f);
            FadePlane(true);
        }

        void FadePlane(bool isVisible)
        {
            if (m_FadeDotsCoroutine != null)
                StopCoroutine(m_FadeDotsCoroutine);

            if (m_FadeAlphaCoroutine != null)
                StopCoroutine(m_FadeAlphaCoroutine);

            m_FadeAlphaCoroutine = StartCoroutine(FadeAlpha(isVisible));
            m_FadeDotsCoroutine = StartCoroutine(ResizeDotsRadius(isVisible));
        }

        IEnumerator ResizeDotsRadius(bool isVisible)
        {
            yield return new WaitForSeconds(m_AlphaFadeDuration);

            var currentRadius = m_Renderer.material.GetFloat(k_DotViewRadius);

            // Resizes material's dot viewing radius from zero to full size, or from full size to zero.
            if (!isVisible)
            {
                while (currentRadius > 0f)
                {
                    currentRadius -= Time.deltaTime / m_DotFadeDuration;
                    m_Renderer.material.SetFloat(k_DotViewRadius, currentRadius);
                    yield return null;
                }

                m_Renderer.material.SetFloat(k_DotViewRadius, 0f);
            }
            else
            {
                while (currentRadius < m_MaximumDotViewRadius)
                {
                    currentRadius += Time.deltaTime / m_DotFadeDuration;
                    m_Renderer.material.SetFloat(k_DotViewRadius, currentRadius);
                    yield return null;
                }

                m_Renderer.material.SetFloat(k_DotViewRadius, m_MaximumDotViewRadius);
            }
        }

        IEnumerator FadeAlpha(bool isVisible)
        {
            var currentAlpha = m_Renderer.material.GetFloat(k_Alpha);

            // Fades material's alpha from zero to maximum or from maximum to zero
            if (!isVisible)
            {
                while (currentAlpha > 0f)
                {
                    currentAlpha -= Time.deltaTime / m_AlphaFadeDuration;
                    m_Renderer.material.SetFloat(k_DotViewRadius, currentAlpha);
                    yield return null;
                }

                m_Renderer.material.SetFloat(k_Alpha, 0f);
            }
            else
            {
                while (currentAlpha < m_MaximumAlpha)
                {
                    currentAlpha += Time.deltaTime / m_AlphaFadeDuration;
                    m_Renderer.material.SetFloat(k_Alpha, currentAlpha);
                    yield return null;
                }

                m_Renderer.material.SetFloat(k_Alpha, m_MaximumAlpha);
            }
        }
    }
}
