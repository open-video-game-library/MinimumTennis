using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ReplayCamera : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject trackingBall;

    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTrackingObject(GameObject trackingObject)
    {
        trackingBall = trackingObject;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = trackingBall.transform;
        virtualCamera.LookAt = trackingBall.transform;
    }
}
