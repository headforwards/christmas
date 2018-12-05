using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface {

	public void UserDetected(long userId, int userIndex)
    {
        // as an example - detect these user specific gestures
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
        manager.DetectGesture(userId, KinectGestures.Gestures.Wave);

        EventManager.TriggerEvent(EventNames.PlayerFound, userIndex.ToString());
    }
	
	public void UserLost(long userId, int userIndex)
    {
		EventManager.TriggerEvent(EventNames.PlayerLost, userIndex.ToString());
    }

	public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        switch (gesture)
        {
            case KinectGestures.Gestures.RaiseRightHand:
                EventManager.TriggerEvent(EventNames.PlayerRaisedRightHand, userIndex.ToString());
                break;
            case KinectGestures.Gestures.Wave:
                EventManager.TriggerEvent(EventNames.PlayerWaving, userIndex.ToString());
                break;
        }

        return true;
    }

	 public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
		//
	}

	 public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        return false;
    }
}
