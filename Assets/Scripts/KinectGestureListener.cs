using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{

    public void UserDetected(long userId, int userIndex)
    {
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.Tpose);
        manager.DetectGesture(userId, KinectGestures.Gestures.Wave);

        EventManager.TriggerEvent(EventNames.PlayerFound, userIndex.ToString());

        EventManager.TriggerEvent(EventNames.DebugMessage, string.Format("player index {0} detected", userIndex));
    }

    public void UserLost(long userId, int userIndex)
    {
        EventManager.TriggerEvent(EventNames.PlayerLost, userIndex.ToString());

        EventManager.TriggerEvent(EventNames.DebugMessage, string.Format("player index {0} lost", userIndex));
    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        switch (gesture)
        {
            case KinectGestures.Gestures.Tpose:
                EventManager.TriggerEvent(EventNames.PlayerReady, userIndex.ToString());
                break;
            case KinectGestures.Gestures.Wave:
                EventManager.TriggerEvent(EventNames.PlayerWaving, userIndex.ToString());
                break;
        }

        EventManager.TriggerEvent(EventNames.DebugMessage, string.Format("{0} complete", gesture));

        return true;
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                             float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        if (gesture == KinectGestures.Gestures.Wave)
        {
            string message = string.Format("{0} - player idx: {4} - x:{1:F0} y:{2:F0} z:{3:F0} degrees", gesture, screenPos.x, screenPos.y, screenPos.z, userIndex);
            EventManager.TriggerEvent(EventNames.DebugMessage, message);
        }
    }

    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                 KinectInterop.JointType joint)
    {
        return false;
    }
}
