using System.Collections;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GamepadInputManager : MonoBehaviour
{
    [NonSerialized]
    public bool isConnected = false;

    [NonSerialized]
    public Vector2 leftStickValue;
    [NonSerialized]
    public Vector2 leftStickAbsValue;

    private bool isPressedEastThisFrame = false;
    [NonSerialized]
    public bool isPressedEast = false;
    [NonSerialized]
    public bool isPressedSouth = false;
    [NonSerialized]
    public bool isPressedWest = false;
    [NonSerialized]
    public bool isPressedNorth = false;
    [NonSerialized]
    public bool isAnyButtonPressed = false;

    private readonly int defaultExtensionFrame = 30;

    void Start()
    {
        isConnected = Gamepad.current != null;
    }

    void Update()
    {
        isConnected = Gamepad.current != null;
        if (!isConnected) return;
        
        isPressedEastThisFrame = Gamepad.current.buttonEast.wasPressedThisFrame;
        if (Gamepad.current.buttonEast.wasPressedThisFrame) { ExtendInputEastFrame(); }
        else if (Gamepad.current.buttonSouth.wasPressedThisFrame) { ExtendInputSouthFrame(); }
        else if (Gamepad.current.buttonWest.wasPressedThisFrame) { ExtendInputWestFrame(); }
        else if (Gamepad.current.buttonNorth.wasPressedThisFrame) { ExtendInputNorthFrame(); }
        isAnyButtonPressed = isPressedEast || isPressedSouth || isPressedWest || isPressedNorth;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            leftStickValue.x = Gamepad.current.leftStick.ReadValue().x;
            leftStickValue.y = -Gamepad.current.leftStick.ReadValue().y;
        }
        else { leftStickValue = Gamepad.current.leftStick.ReadValue(); }

        leftStickAbsValue.x = Mathf.Abs(leftStickValue.x);
        leftStickAbsValue.y = Mathf.Abs(leftStickValue.y);
    }

    public bool InputEastThisFrame()
    {
        bool returnValue = isPressedEastThisFrame;
        isPressedEastThisFrame = false;
        isPressedEast = false;
        return returnValue;
    }

    private void ExtendInputEastFrame()
    {
        isPressedEast = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedEast = false; }));
    }

    private void ExtendInputSouthFrame()
    {
        isPressedSouth = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedSouth = false; }));
    }

    private void ExtendInputWestFrame()
    {
        isPressedWest = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedWest = false; }));
    }

    private void ExtendInputNorthFrame()
    {
        isPressedNorth = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedNorth = false; }));
    }

    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++) { yield return null; }
        action();
    }
}
