using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCameraController : MonoBehaviour
{
    // Put in the object you want to focus on from Inspector.
    private GameObject targetObject = null; 

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null) { targetObject = GameObject.FindGameObjectWithTag("Ball"); }
        else if (targetObject.CompareTag("Ball"))
        {
            // Determine the speed of completion
            float speed = 0.10f;
            // Obtain a vector in the target direction
            Vector3 relativePos = targetObject.transform.position - transform.position;
            // Converts direction to rotation information
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            // Complement the current rotation information with the rotation information in the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
        }
    }
}
