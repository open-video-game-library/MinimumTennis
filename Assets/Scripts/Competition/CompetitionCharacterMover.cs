using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CompetitionCharacterMover : MonoBehaviour
{
    public GameObject[] players;
    public GameObject gameManager;

    private CompetitionGameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameManager.GetComponent<CompetitionGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            float nextX = players[i].transform.position.x + (2.0f * i - 1.0f) * 0.50f * Gamepad.all[i].leftStick.ReadValue().y;
            float nextY = players[i].transform.position.y;
            float nextZ = players[i].transform.position.z - (2.0f * i - 1.0f) * 0.50f * Gamepad.all[i].leftStick.ReadValue().x;

            if (!CheckPlayerPosition(new Vector3(nextX, nextY, nextZ), i)) { return; }
            if (!CheckGameStatus(manager)) { return; }

            if (Gamepad.all[i].leftStick.ReadValue().magnitude > 0.10f)
            {
                if (Gamepad.all[i].leftStick.ReadValue().y <= 0.0f)
                {
                    players[i].transform.Translate(-0.25f * Gamepad.all[i].leftStick.ReadValue().y,
                    0.0f, 0.50f * Gamepad.all[i].leftStick.ReadValue().x);
                }
                else
                {
                    players[i].transform.Translate(-0.50f * Gamepad.all[i].leftStick.ReadValue().y,
                    0.0f, 0.50f * Gamepad.all[i].leftStick.ReadValue().x);
                }
            }
        }
    }

    private bool CheckPlayerPosition(Vector3 playerPosition, int p)
    {
        float x = (-2.0f * p + 1.0f) * playerPosition.x;
        float y = playerPosition.y;
        float z = playerPosition.z;

        bool checkX = (2.0f <= x) && (x <= 80.0f);
        bool checkY = y >= 0.0f;
        bool checkZ = (-34.0f <= z) && (z <= 34.0f);

        return checkX && checkY && checkZ;
    }

    private bool CheckGameStatus(CompetitionGameManager manager)
    {
        bool active = manager.active;
        bool serveStatus = manager.isTOS;
        bool ballCount = manager.Check("Ball");

        return active && !serveStatus && !ballCount;
    }
}
