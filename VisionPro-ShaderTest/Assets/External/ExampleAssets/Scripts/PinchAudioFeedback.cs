using System.Collections;
using UnityEngine;

namespace PolySpatial.Template
{
    public class PinchAudioFeedback : MonoBehaviour
    {
        [SerializeField]
        AudioSource m_AudioSource;

        bool m_DelayBool = false;
        Coroutine m_DelayCoroutine;

        public void PlayAudioPinchStart()
        {
            m_AudioSource.volume = 0.5f;
            m_AudioSource.Play();

            if (m_DelayCoroutine != null)
                StopCoroutine(m_DelayCoroutine);

            m_DelayCoroutine = StartCoroutine(Delay(0.5f));
        }

        IEnumerator Delay(float time)
        {
            m_DelayBool = true;
            yield return new WaitForSeconds(time);
            m_DelayBool = false;
        }

        public void PlayAudioPinchEnd()
        {
            if (m_DelayBool)
                return;

            m_AudioSource.volume = 1f;
            m_AudioSource.Play();
        }
    }
}
