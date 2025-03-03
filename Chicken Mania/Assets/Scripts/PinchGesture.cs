using System;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Pointers;
using UnityEngine;

public class PinchGesture : Gesture
{
    public float PinchDistance { get; private set; }

    private float initialDistance = 0f;
    private bool isPinching = false;
    private Pointer firstPointer, secondPointer;

    protected override void pointersPressed(IList<Pointer> pointers)
    {
        if (State == GestureState.Possible && ActivePointers.Count == 2)
        {
            firstPointer = ActivePointers[0];
            secondPointer = ActivePointers[1];
            initialDistance = GetFingerDistance();
            isPinching = true;
            setState(GestureState.Began);
        }
    }

    protected override void pointersUpdated(IList<Pointer> pointers)
    {
        if (isPinching && ActivePointers.Count == 2)
        {
            PinchDistance = GetFingerDistance() - initialDistance;

            if (Mathf.Abs(PinchDistance) > 10f) // Threshold to detect a pinch
            {
                setState(GestureState.Changed);
            }
        }
    }

    protected override void pointersReleased(IList<Pointer> pointers)
    {
        if (ActivePointers.Count < 2)
        {
            isPinching = false;
            setState(GestureState.Ended);
        }
    }

    private float GetFingerDistance()
    {
        if (ActivePointers.Count < 2) return 0f;
        return Vector2.Distance(firstPointer.Position, secondPointer.Position);
    }
}
