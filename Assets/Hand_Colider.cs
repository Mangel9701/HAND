using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Colider : MonoBehaviour
{
    #region Singleton
    private static Hand_Colider _instance;

    public static Hand_Colider Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    #endregion

    private TrackingInfo tracking;
    public Vector3 currentPosition;

    void Start()
    {
        gameObject.tag = "Player";
    }


    void Update()
    {
        tracking = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        currentPosition = Camera.main.ViewportToWorldPoint(new Vector3(tracking.palm_center.x, tracking.palm_center.y, tracking.depth_estimation));
        transform.position = currentPosition;
    }
}
