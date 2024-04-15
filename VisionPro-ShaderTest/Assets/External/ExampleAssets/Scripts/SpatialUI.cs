using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PolySpatial.Template
{
    public class SpatialUI : MonoBehaviour
    {
        [SerializeField]
        AudioSource m_AudioSource;

        [SerializeField]
        bool m_ScaleBackground;

        [SerializeField]
        protected Color m_SelectedColor = new(.1254f, .5882f, .9529f);

        [SerializeField]
        protected Color m_UnselectedColor = new(.1764f, .1764f, .1764f);

        [SerializeField]
        protected UnityEvent m_PressStart;

        [SerializeField]
        protected UnityEvent m_PressEnd;

        bool m_Delay;
        Coroutine m_DelayCoroutine;
        Vector3 m_StartingScale;
        // TODO Hover component?

        public UnityEvent PressEndEvent => m_PressEnd;

        void Awake()
        {
            if (m_ScaleBackground)
                m_StartingScale = transform.localScale;
        }

        public virtual void PressStart()
        {
            if (m_AudioSource != null)
                PlayAudioPinchStart();

            if (m_ScaleBackground)
                SetScale(0.95f);
        }

        public virtual void PressEnd()
        {
            if (m_AudioSource != null)
                PlayAudioPinchEnd();

            if (m_ScaleBackground)
                SetScale(1f);
        }

        void PlayAudioPinchStart()
        {
            m_AudioSource.volume = 0.25f;
            m_AudioSource.Play();

            if (m_DelayCoroutine != null)
                StopCoroutine(m_DelayCoroutine);

            m_DelayCoroutine = StartCoroutine(Delay(0.5f));
        }

        void SetScale(float scale)
        {
            transform.localScale = m_StartingScale * scale;
        }

        IEnumerator Delay(float time)
        {
            m_Delay = true;
            yield return new WaitForSeconds(time);
            m_Delay = false;
        }

        void PlayAudioPinchEnd()
        {
            if (m_Delay)
                return;

            m_AudioSource.volume = 1f;
            m_AudioSource.Play();
        }
    }
}
