using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    private float playerX;
    private float playerZ;

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;

        player = GameObject.Find("Player");
        offset = transform.position - player.transform.position;
        playerX = player.transform.position.x;
        playerZ = player.transform.position.z;
        transform.Translate(transform.position.x - playerX, 0.0f, transform.position.z - playerZ);
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, 5.0f * Time.deltaTime);
    }
}
