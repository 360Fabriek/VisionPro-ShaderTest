using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PolySpatial.Template
{
    /// <summary>
    /// An interactable that aligns/faces the position of the interactor
    /// </summary>
    public class XRBlaster : XRGrabInteractable
    {
        IXRSelectInteractor m_Interactor;
        bool m_Front;

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
            m_Interactor = args.interactorObject;
            var interactorTransform = m_Interactor.GetAttachTransform(this);
            var posRelative = transform.InverseTransformPoint(interactorTransform.position);
            m_Front = posRelative.z > 0;

            UpdateRotation();
        }

        void EndGrab(SelectExitEventArgs args)
        {
            m_Interactor = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected)
                {
                    UpdateRotation();
                }
            }
        }

        void UpdateRotation()
        {
            var interactorTransform = m_Interactor.GetAttachTransform(this);

            if (m_Front)
            {
                transform.LookAt(interactorTransform.position);
            }
            else
            {
                transform.LookAt(interactorTransform.position);
                transform.rotation *= Quaternion.Euler(new Vector3(0, 180f, 0f));
            }
        }
    }
}
