using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.ARFoundation;

public class SessionStatusTracking : MonoBehaviour
{

    private TMPro.TextMeshProUGUI sessionStatusText;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        sessionStatusText = this.GetComponent<TMPro.TextMeshProUGUI>();
        sessionStatusText.text = "Hello";
        ManomotionManager.OnManoMotionFrameProcessed += handleMotionFrame;
        //ARSession.stateChanged += HandleStateChanged;
    }

    void handleMotionFrame()
    {
        if (ManomotionManager.Instance.Hand_infos.Length > 0)
        {
           HandInfoUnity handInfoUnity = ManomotionManager.Instance.Hand_infos[0];
           TrackingInfo trackingInfo = handInfoUnity.hand_info.tracking_info;
           GestureInfo gestureInfo = handInfoUnity.hand_info.gesture_info;
            if (gestureInfo.mano_gesture_trigger == ManoGestureTrigger.GRAB_GESTURE)
            {
                sessionStatusText.text = "Grab" + count++;
            } else {
                sessionStatusText.text = "Nothing -" + gestureInfo.mano_gesture_trigger + count;
            }
        } else {
            sessionStatusText.text = "In else";
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     try {
    //         if (ManomotionManager.Instance.Hand_infos.Length > 0)
    //         {
    //             HandInfoUnity handInfoUnity = ManomotionManager.Instance.Hand_infos[0];
    //             sessionStatusText.text = handInfoUnity.hand_info.gesture_info.mano_class.ToString();
    //         } else
    //         {
    //             sessionStatusText.text = "No State atm";
    //         }
    //     } catch(Exception ex) {
    //         sessionStatusText.text = ex.Message;
    //     }
    // }

    //private void HandleStateChanged(ARSessionStateChangedEventArgs stateEventArguments)
    //{
    //    sessionStatusText.text = stateEventArguments.state.ToString();
    //}
}