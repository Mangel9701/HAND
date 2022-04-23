using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHasSpawn : MonoBehaviour
{
    public static event EventHandler<OnPlaneSpawnArgs> onPlaneSpawn;
    public class OnPlaneSpawnArgs : EventArgs
    {
        public Transform transform;
    }
    private void Awake()
    {
        onPlaneSpawn?.Invoke(this, new OnPlaneSpawnArgs { transform = gameObject.transform });
    }
}
