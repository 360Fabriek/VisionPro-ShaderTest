using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

namespace PolySpatial.Template
{
    public class ARPlaneInteractable : XRBaseInteractable
    {
        [SerializeField]
        float m_RayCastMaxDistance = 2f;

        [SerializeField]
        LayerMask m_LayerMask = int.MaxValue;

        [SerializeField]
        GameObject m_GuidePrefab;

        [SerializeField]
        GameObject m_GuideErrorPrefab;

        [SerializeField]
        float m_OffsetY;

        [SerializeField]
        AnimationCurve m_AnimationCurve;
        
        bool m_Colliding;
        Quaternion m_StartingRotation;
        Vector3 m_StartingPosition;
        Vector3 m_StartingOffset;
        Vector3 m_Velocity = Vector3.zero;
        Vector3 m_PlaneHitPoint;
        XRHandSubsystem m_HandSubsystem;
        XRHandJoint m_RightIndexTipJoint;
        XRHandJoint m_LeftIndexTipJoint;
        float m_StartYaw;
        Vector3 m_StartDirection;
        int m_LastInteractorCount;
        GameObject m_GuideTemp;
        GameObject m_GuideErrorTemp;
        VerticalGuide m_VerticalGuide;
        Vector3 m_NonSnappingPosition = Vector3.zero;

        void Start()
        {
            GetHandSubsystem();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        void StartGrab(SelectEnterEventArgs args)
        {
            if (interactorsSelecting.Count == 1)
            {
                var interactor = args.interactorObject;
                var interactable = args.interactableObject;
                var interactorPosition = interactor.GetAttachTransform(this).position;
                var interactablePosition = interactable.transform.position;
                m_StartingPosition = interactablePosition;
                m_StartingRotation = interactable.transform.rotation;
                m_StartingOffset = interactorPosition - interactablePosition;
            }
        }

        void EndGrab(SelectExitEventArgs args)
        {
            if (interactorsSelecting.Count > 0)
            {
                var interactor = interactorsSelecting[0];
                var interactable = args.interactableObject;
                var interactorPosition = interactor.GetAttachTransform(this).position;
                var interactablePosition = interactable.transform.position;
                m_StartingOffset = interactorPosition - interactablePosition;
                return;
            }

            if (!m_Colliding)
            {
                transform.position = m_StartingPosition;
                if (m_LastInteractorCount < 2)
                    transform.rotation = m_StartingRotation;
            }

            if (m_GuideTemp != null)
            {
                var guideTempPosition = m_GuideTemp.transform.position;
                var thisTransformPosition = transform.position;
                var groundPosition = new Vector3(guideTempPosition.x, guideTempPosition.y + m_OffsetY, guideTempPosition.z);
                var sec = Mathf.Sqrt(2f * Vector3.Distance(thisTransformPosition, guideTempPosition) / 9.8f);
                StartCoroutine(SmoothLerp(sec, thisTransformPosition, groundPosition));
                Destroy(m_GuideTemp);
            }

            if (m_GuideErrorTemp != null)
            {
                Destroy(m_GuideErrorTemp);
            }

            m_Colliding = false;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic) return;
            if (!isSelected) return;

            if (HasMultipleInteractors())
                RotateAroundYAxis();
            else
            {
                var interactor = interactorsSelecting[0];
                var interactorTransform = interactor.GetAttachTransform(this);
                var interactorTransformPosition = interactorTransform.position;
                m_NonSnappingPosition = interactorTransformPosition - m_StartingOffset;

                if (CheckIfSnappingOnPlane(0.25f))
                {
                    var target = interactorTransformPosition - m_StartingOffset;
                    var groundPosition = new Vector3(target.x, m_PlaneHitPoint.y + m_OffsetY, target.z);
                    transform.position = Vector3.SmoothDamp(transform.position, groundPosition, ref m_Velocity, 0.05f);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position, interactorTransformPosition - m_StartingOffset, ref m_Velocity, 0.05f);
                }
            }

            if (CheckIfOnPlane())
            {
                if (!m_Colliding)
                {
                    m_GuideTemp = Instantiate(m_GuidePrefab);
                    m_VerticalGuide = m_GuideTemp.GetComponent<VerticalGuide>();
                    Destroy(m_GuideErrorTemp);
                }

                m_Colliding = true;
            }
            else
            {
                if (!m_Colliding)
                    Destroy(m_GuideTemp);

                if (m_Colliding)
                    m_GuideErrorTemp = Instantiate(m_GuideErrorPrefab);

                m_Colliding = false;
            }

            m_LastInteractorCount = interactorsSelecting.Count;
        }

        bool HasMultipleInteractors()
        {
            return interactorsSelecting.Count > 1;
        }

        void GetHandSubsystem()
        {
            var xrGeneralSettings = XRGeneralSettings.Instance;
            if (xrGeneralSettings == null)
                Debug.LogError("XR general settings not set");

            var manager = xrGeneralSettings.Manager;
            if (manager != null)
            {
                var loader = manager.activeLoader;
                if (loader != null)
                {
                    m_HandSubsystem = loader.GetLoadedSubsystem<XRHandSubsystem>();
                    if (!CheckHandSubsystem())
                        return;

                    m_HandSubsystem.Start();
                }
            }
        }

        bool CheckHandSubsystem()
        {
            if (m_HandSubsystem == null)
            {
                Debug.LogError("Could not find Hand Subsystem");
                enabled = false;
                return false;
            }

            return true;
        }

        void RotateAroundYAxis()
        {
            if (!CheckHandSubsystem())
                return;

            m_RightIndexTipJoint = m_HandSubsystem.rightHand.GetJoint(XRHandJointID.IndexTip);
            m_LeftIndexTipJoint = m_HandSubsystem.leftHand.GetJoint(XRHandJointID.IndexTip);

            if (m_RightIndexTipJoint.TryGetPose(out var indexPose1) && m_LeftIndexTipJoint.TryGetPose(out var indexPose2))
                CalcRotation(indexPose1.position, indexPose2.position);
        }

        bool CheckIfSnappingOnPlane(float snappingDistance)
        {
            if (Physics.Raycast(m_NonSnappingPosition, Vector3.down, out var hit, snappingDistance, m_LayerMask))
            {
                if (hit.collider != null)
                {
                    m_PlaneHitPoint = hit.point;
                    return true;
                }
            }

            return false;
        }

        bool CheckIfOnPlane()
        {
            var thisTransform = transform;
            var thisTransformPosition = thisTransform.position;
            if (Physics.Raycast(thisTransformPosition, Vector3.down, out var hit, m_RayCastMaxDistance, m_LayerMask))
            {
                if (hit.collider != null)
                {
                    if (m_GuideTemp != null)
                    {
                        var guideTempTransform = m_GuideTemp.transform;
                        var guideTempEulerAngles = guideTempTransform.rotation.eulerAngles;
                        guideTempTransform.position = hit.point;
                        guideTempTransform.localEulerAngles = new Vector3(guideTempEulerAngles.x, thisTransform.rotation.eulerAngles.y, guideTempEulerAngles.z);
                        m_VerticalGuide.SetGuideHeight(Vector3.Distance(thisTransformPosition, hit.point));
                    }

                    return true;
                }
            }

            if (m_GuideErrorTemp != null)
            {
                var guidePosition = new Vector3(thisTransformPosition.x, thisTransformPosition.y - m_OffsetY, thisTransformPosition.z);
                var guideErrorTransform = m_GuideErrorTemp.transform;
                guideErrorTransform.position = guidePosition;
                guideErrorTransform.localEulerAngles = thisTransform.localEulerAngles;
            }

            return false;
        }

        void CalcRotation(Vector3 thisPosition, Vector3 otherPosition)
        {
            var rayToRay = otherPosition - thisPosition;
            rayToRay.y = 0;

            if (m_LastInteractorCount == 1)
            {
                //set start yaw and direction if its the first frame of two handed rotation
                m_StartYaw = transform.rotation.eulerAngles.y;
                m_StartDirection = rayToRay;
            }

            var yawSign = Mathf.Sign(Vector3.Dot(Quaternion.AngleAxis(90f, Vector3.down) * m_StartDirection, rayToRay));
            var currentYaw = m_StartYaw - Vector3.Angle(m_StartDirection, rayToRay) * yawSign;
            var currentRotation = Quaternion.AngleAxis(currentYaw, Vector3.up);
            transform.rotation = currentRotation;
        }

        IEnumerator SmoothLerp(float duration, Vector3 startingPosition, Vector3 finalPosition)
        {
            var increment = 0f;
            while (increment <= duration)
            {
                increment += Time.deltaTime;
                var percent = Mathf.Clamp01(increment / duration);
                var curvePercent = m_AnimationCurve.Evaluate(percent);
                var newPosition = Vector3.Lerp(startingPosition, finalPosition, curvePercent);
                transform.position = newPosition;
                yield return null;
            }
        }
    }
}
