using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mano_gestures : MonoBehaviour
{
    ManoGestureContinuous click;
    // Start is called before the first frame update
    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == click) {

             transform.parent = other.gameObject.transform;
            Handheld.Vibrate();
        
        }
            else
            { transform.parent = null; }
         
        
    }

    void initialize()
    {
        click = ManoGestureContinuous.CLOSED_HAND_GESTURE;

        
    }
}
