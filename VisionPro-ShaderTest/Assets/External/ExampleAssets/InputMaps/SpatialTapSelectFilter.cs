using System.Collections;
using System.Collections.Generic;
using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SpatialTapSelectFilter : MonoBehaviour, IXRSelectFilter
{

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {

        var activeTouches = Touch.activeTouches;
        if (activeTouches.Count > 0)
        {
            var primaryTouchData = EnhancedSpatialPointerSupport.GetPointerState(activeTouches[0]);

            return primaryTouchData.Kind == SpatialPointerKind.IndirectPinch || primaryTouchData.Kind == SpatialPointerKind.DirectPinch;
        }

        return false;
    }

    public bool canProcess => isActiveAndEnabled;
}
