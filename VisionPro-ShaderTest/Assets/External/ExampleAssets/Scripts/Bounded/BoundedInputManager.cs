using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace PolySpatial.Template
{
    public class BoundedInputManager : MonoBehaviour
    {
        [SerializeField]
        bool m_UsePhysics = true;

        [SerializeField]
        AudioSource m_AudioSource;

        [SerializeField]
        float m_Delay = 0.5f;

        TouchPhase m_LastTouchPhase;
        BoundedObjectBehavior m_SelectedObject;

        bool m_PlayedEndAudio;
        bool m_ActiveDirectPinch;
        float m_RealTimeAtPinch;

        const float k_StartAudioVolume = 0.5f;
        const float k_EndAudioVolume = 1f;

        void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        void Update()
        {
            var activeTouches = Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
                var primaryTouchData = EnhancedSpatialPointerSupport.GetPointerState(activeTouches[0]);
                var touchPhase = activeTouches[0].phase;

                if (touchPhase == TouchPhase.Began && primaryTouchData.Kind == SpatialPointerKind.IndirectPinch || primaryTouchData.Kind == SpatialPointerKind.DirectPinch)
                {
                    if (primaryTouchData.targetObject != null)
                    {
                        if(primaryTouchData.targetObject.TryGetComponent(out BoundedObjectBehavior boundedObject))
                        {
                            m_SelectedObject = boundedObject;
                            m_SelectedObject.Select(true);

                            if (m_AudioSource != null && !m_ActiveDirectPinch)
                            {
                                m_AudioSource.volume = k_StartAudioVolume;
                                m_AudioSource.Play();
                                m_RealTimeAtPinch = Time.realtimeSinceStartup;
                                m_PlayedEndAudio = false;
                            }
                            m_ActiveDirectPinch = primaryTouchData.Kind == SpatialPointerKind.DirectPinch;
                        }
                    }
                }

                if (touchPhase == TouchPhase.Moved)
                {
                    if (m_SelectedObject != null)
                    {
                        // use physics and move object for non direct pinch (interaction kind 1)
                        if (m_UsePhysics && primaryTouchData.Kind == SpatialPointerKind.IndirectPinch)
                        {
                            m_SelectedObject.MoveWithPhysics(primaryTouchData.interactionPosition);
                        }
                        else
                        {
                            m_SelectedObject.MoveDirectly(primaryTouchData);
                        }
                    }
                }

                if (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled)
                {
                    if (m_SelectedObject != null)
                    {
                        m_SelectedObject.Select(false);
                        m_SelectedObject = null;
                        m_ActiveDirectPinch = false;
                        m_PlayedEndAudio = true;
                    }
                }
            }

            if (m_RealTimeAtPinch + m_Delay <= Time.realtimeSinceStartup && m_PlayedEndAudio)
            {
                m_PlayedEndAudio = false;
                m_AudioSource.volume = k_EndAudioVolume;
                m_AudioSource.Play();
            }
        }
    }
}
