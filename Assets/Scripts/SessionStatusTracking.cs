using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.ARFoundation;

public class SessionStatusTracking : MonoBehaviour
{

    private TMPro.TextMeshProUGUI sessionStatusText;

    // Start is called before the first frame update
    void Start()
    {
        sessionStatusText = this.GetComponent<TMPro.TextMeshProUGUI>();
        sessionStatusText.text = "Hello";
        //ARSession.stateChanged += HandleStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        try {
            if (ManomotionManager.Instance.Hand_infos.Length > 0)
            {
                HandInfoUnity handInfoUnity = ManomotionManager.Instance.Hand_infos[0];
                sessionStatusText.text = handInfoUnity.hand_info.gesture_info.mano_class.ToString();
            } else
            {
                sessionStatusText.text = "No State atm";
            }
        } catch(Exception ex) {
            sessionStatusText.text = ex.Message;
        }
    }

    //private void HandleStateChanged(ARSessionStateChangedEventArgs stateEventArguments)
    //{
    //    sessionStatusText.text = stateEventArguments.state.ToString();
    //}
}