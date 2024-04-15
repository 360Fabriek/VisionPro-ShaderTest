using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace PolySpatial.Template
{
    public class ButtonInputManager : MonoBehaviour
    {
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

                if (activeTouches[0].phase == TouchPhase.Began)
                {
                    if (primaryTouchData.targetObject.TryGetComponent(out LoadLevelButton button))
                    {
                        button.LoadLevel();
                    }
                }
            }

        }
    }
}
