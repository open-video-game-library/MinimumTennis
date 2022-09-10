using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProControllerManager : MonoBehaviour
{
    [System.NonSerialized] public bool isPressedEast = false;
    [System.NonSerialized] public bool isPressedSouth = false;
    [System.NonSerialized] public bool isPressedWest = false;
    [System.NonSerialized] public bool isPressedNorth = false;

    void Update()
    {
        // "null" if no gamepad is connected.
        if (Gamepad.current == null) return;

        // Only once per frame to determine if the button is pressed.
        if (Gamepad.current.buttonEast.wasPressedThisFrame) { isPressedEast = true; }
        if (Gamepad.current.buttonSouth.wasPressedThisFrame) { isPressedSouth = true; }
        if (Gamepad.current.buttonWest.wasPressedThisFrame) { isPressedWest = true; }
        if (Gamepad.current.buttonNorth.wasPressedThisFrame) { isPressedNorth = true; }
    }

    public bool PressEastButton()
    {
        if (Gamepad.current == null) { return false; }
        else if (Gamepad.current.buttonEast.wasPressedThisFrame) { return isPressedEast; }
        else { return false; }
    }

    public bool PressSouthButton()
    {
        if (Gamepad.current == null) { return false; }
        else if (Gamepad.current.buttonSouth.wasPressedThisFrame) { return isPressedSouth; }
        else { return false; }
    }

    public bool PressWestButton()
    {
        if (Gamepad.current == null) { return false; }
        else if (Gamepad.current.buttonWest.wasPressedThisFrame) { return isPressedWest; }
        else { return false; }
    }

    public bool PressNorthButton()
    {
        if (Gamepad.current == null) { return false; }
        else if (Gamepad.current.buttonNorth.wasPressedThisFrame) { return isPressedNorth; }
        else { return false; }
    }
}
