using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotographCameraController : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speed = new Vector3(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
        transform.Rotate(speed);
    }
}
