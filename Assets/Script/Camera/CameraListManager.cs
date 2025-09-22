using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraListManager : MonoBehaviour
{
    public static CameraListManager instance;

    public List<CinemachineVirtualCamera> cameraList;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        foreach(var camera in cameraList)
        {
            if(camera.Follow == null)
            {
                camera.Follow = CameraFollow.instance.transform;
            }
        }
    }
}
