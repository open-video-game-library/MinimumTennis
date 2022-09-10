using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CompetitionGameControllerManager : MonoBehaviour
{
    [System.NonSerialized] public int gamepadCount;
    [System.NonSerialized] public Vector2[] leftStick;

    [System.NonSerialized] public bool[] isPressedEast;
    [System.NonSerialized] public bool[] isPressedSouth;
    [System.NonSerialized] public bool[] isPressedWest;
    [System.NonSerialized] public bool[] isPressedNorth;

    // Start is called before the first frame update
    void Start()
    {
        gamepadCount = Gamepad.all.Count;
        leftStick = new Vector2[gamepadCount];

        isPressedEast = new bool[gamepadCount];
        isPressedSouth = new bool[gamepadCount];
        isPressedWest = new bool[gamepadCount];
        isPressedNorth = new bool[gamepadCount];
    }

    // Update is called once per frame
    void Update()
    {
        gamepadCount = Gamepad.all.Count;
        Debug.Log(gamepadCount);

        // Not called unless gamepad is connected.
        if (Gamepad.current == null || Gamepad.all.Count > 2) { return; }

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            leftStick[i] = Gamepad.all[i].leftStick.ReadValue();

            // Only once per frame to determine if the button is pressed.
            if (Gamepad.all[i].buttonEast.wasPressedThisFrame) { isPressedEast[i] = true; }
            if (Gamepad.all[i].buttonSouth.wasPressedThisFrame) { isPressedSouth[i] = true; }
            if (Gamepad.all[i].buttonWest.wasPressedThisFrame) { isPressedWest[i] = true; }
            if (Gamepad.all[i].buttonNorth.wasPressedThisFrame) { isPressedNorth[i] = true; }
        }
     }

    public bool PressEastButton(int p)
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (Gamepad.all[i] == null) { return false; }
            if (Gamepad.all[i].buttonEast.wasPressedThisFrame) { return isPressedEast[p]; }
        }
        return false;
    }

    public bool PressSouthButton(int p)
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (Gamepad.all[i] == null) { return false; }
            if (Gamepad.all[i].buttonSouth.wasPressedThisFrame) { return isPressedSouth[p]; }
        }
        return false;
    }

    public bool PressWestButton(int p)
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (Gamepad.all[i] == null) { return false; }
            if (Gamepad.all[i].buttonWest.wasPressedThisFrame) { return isPressedWest[p]; }
        }
        return false;
    }

    public bool PressNorthButton(int p)
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (Gamepad.all[i] == null) { return false; }
            if (Gamepad.all[i].buttonNorth.wasPressedThisFrame) { return isPressedNorth[p]; }
        }
        return false;
    }
}
