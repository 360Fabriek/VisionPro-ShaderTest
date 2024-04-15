using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using Unity.XR.CoreUtils;

namespace PolySpatial.Template
{
    public class TrackedHandManager : MonoBehaviour
    {
        [SerializeField]
        GameObject m_LeftHandPrefab;

        [SerializeField]
        GameObject m_RightHandPrefab;

        [SerializeField]
        GameObject m_FingerTipPrefab;

        [SerializeField]
        bool m_ShowFingerTips;


        GameObject m_SpawnedRightHand;
        GameObject m_SpawnedLeftHand;
        GameObject m_SpawnedLeftHandTip;
        GameObject m_SpawnedRightHandTip;
        XRHandSubsystem m_Subsystem;
        XRHandJoint m_LeftHandJoint;
        XRHandJoint m_RightHandJoint;
        XRHandJoint m_LeftIndexTipJoint;
        XRHandJoint m_RightIndexTipJoint;
        XRHandSubsystem m_HandSubsystem;

        void Start()
        {
            var xrOrigin = FindObjectOfType<XROrigin>();

            m_SpawnedRightHand = Instantiate(m_RightHandPrefab, Vector3.up, Quaternion.identity, xrOrigin.transform);
            m_SpawnedLeftHand = Instantiate(m_LeftHandPrefab, Vector3.up, Quaternion.identity, xrOrigin.transform);

            if (m_ShowFingerTips)
            {
                m_SpawnedLeftHandTip = Instantiate(m_FingerTipPrefab);
                m_SpawnedRightHandTip = Instantiate(m_FingerTipPrefab);
            }

            GetHandSubsystem();
        }

        void Update()
        {
            if (!CheckHandSubsystem())
                return;

            var updateSuccessFlags = m_HandSubsystem.TryUpdateHands(XRHandSubsystem.UpdateType.Dynamic);

            if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose) != 0)
            {
                // assign joint values
                m_RightIndexTipJoint = m_HandSubsystem.rightHand.GetJoint(XRHandJointID.IndexTip);
                m_RightHandJoint = m_HandSubsystem.rightHand.GetJoint(XRHandJointID.MiddleDistal);
            }

            if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose) != 0)
            {
                // assign joint values
                m_LeftIndexTipJoint = m_HandSubsystem.leftHand.GetJoint(XRHandJointID.IndexTip);
                m_LeftHandJoint = m_HandSubsystem.leftHand.GetJoint(XRHandJointID.MiddleDistal);
            }

            TrackHands(m_SpawnedLeftHand, m_LeftHandJoint);
            TrackHands(m_SpawnedRightHand, m_RightHandJoint);
            if (m_ShowFingerTips)
            {
                TrackHands(m_SpawnedLeftHandTip, m_LeftIndexTipJoint);
                TrackHands(m_SpawnedRightHandTip, m_RightIndexTipJoint);
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

        void GetHandSubsystem()
        {
            var xrGeneralSettings = XRGeneralSettings.Instance;
            if (xrGeneralSettings == null)
            {
                Debug.LogError("XR general settings not set");
                return;
            }

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

        static void TrackHands(GameObject spawnedObject, XRHandJoint joint)
        {
            if (joint.TryGetPose(out var pose))
            {
                spawnedObject.transform.position = pose.position;
                spawnedObject.transform.rotation = pose.rotation;
            }
        }
    }
}
