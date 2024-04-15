using UnityEngine;

#if INCLUDE_UNITY_XRI
using UnityEngine.XR.Interaction.Toolkit.UI;
#else
using UnityObject = UnityEngine.Object;
#endif

#if UNITY_EDITOR || UNITY_VISIONOS
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.VisionOS.InputDevices;
#endif

namespace UnityEngine.XR.VisionOS.Samples.URP
{
    public class AnchorPlacer : MonoBehaviour
    {
        const int k_FirstPointerId = 0;
        const int k_SecondPointerId = 1;

        [SerializeField]
        GameObject m_AnchorPrefab;

        [SerializeField]
#if INCLUDE_UNITY_XRI
        XRUIInputModule m_InputModule;
#else
        UnityObject m_InputModule;
#endif

#if UNITY_EDITOR || UNITY_VISIONOS
        ARAnchor m_Anchor;
        PointerInput m_PointerInput;

        void OnEnable()
        {
            m_PointerInput ??= new PointerInput();
            m_PointerInput.Enable();
        }

        void OnDisable()
        {
            m_PointerInput.Disable();
        }

        void Update()
        {
            // Wait until session is ready and tracking
            if (ARSession.state < ARSessionState.SessionTracking)
            {
                return;
            }

            var primaryTouch = m_PointerInput.Default.PrimaryPointer.ReadValue<VisionOSSpatialPointerState>();
            if (primaryTouch.phase != VisionOSSpatialPointerPhase.Began)
                return;

#if INCLUDE_UNITY_XRI
            // Don't place anchors when the user is interacting with UI
            if (m_InputModule.IsPointerOverGameObject(k_FirstPointerId) || m_InputModule.IsPointerOverGameObject(k_SecondPointerId))
                return;
#endif

            if (m_Anchor != null)
                Destroy(m_Anchor.gameObject);

            var anchorGameObject = Instantiate(m_AnchorPrefab);
            anchorGameObject.name = $"Anchor {Time.time}";
            var anchorTransform = anchorGameObject.transform;
            var thisTransform = transform;
            anchorTransform.parent = thisTransform;

            var rayOrigin = thisTransform.TransformPoint(primaryTouch.startRayOrigin);
            var rayDirection = thisTransform.TransformVector(primaryTouch.startRayDirection);
            var ray = new Ray(rayOrigin, rayDirection);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                anchorTransform.SetPositionAndRotation(hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            }
            else
            {
                anchorTransform.SetLocalPositionAndRotation(primaryTouch.inputDevicePosition, primaryTouch.inputDeviceRotation);
            }

            m_Anchor = anchorTransform.gameObject.AddComponent<ARAnchor>();
        }
#endif
    }
}
