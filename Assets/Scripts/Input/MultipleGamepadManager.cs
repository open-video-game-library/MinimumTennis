using System.Collections;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class MultipleGamepadManager : MonoBehaviour
{
    [NonSerialized]
    public int gamepadCount;
    [NonSerialized]
    public bool[] isConnected;

    [NonSerialized]
    public Vector2[] leftStickValue;
    [NonSerialized]
    public Vector2[] leftStickAbsValue;

    private bool[] isPressedEastThisFrame;
    [NonSerialized]
    public bool[] isPressedEast;
    [NonSerialized]
    public bool[] isPressedSouth;
    [NonSerialized]
    public bool[] isPressedWest;
    [NonSerialized]
    public bool[] isPressedNorth;
    [NonSerialized]
    public bool[] isAnyButtonPressed;

    private readonly int playerNum = 2;
    private readonly int defaultExtensionFrame = 30;

    // Start is called before the first frame update
    void Start()
    {
        gamepadCount = playerNum;
        isPressedEastThisFrame = new bool[gamepadCount];
        leftStickValue = new Vector2[gamepadCount];
        leftStickAbsValue = new Vector2[gamepadCount];

        isConnected = new bool[gamepadCount];
        isPressedEast = new bool[gamepadCount];
        isPressedSouth = new bool[gamepadCount];
        isPressedWest = new bool[gamepadCount];
        isPressedNorth = new bool[gamepadCount];
        isAnyButtonPressed = new bool[gamepadCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count < playerNum)
        {
            Debug.Log("The number of controllers currently connected is " + Gamepad.all.Count + ". To play this game, please connect the two controllers to your PC.");
            return;
        }
        
        for (int i = 0; i < gamepadCount; i++)
        {
            isConnected[i] = Gamepad.all[i] != null;

            isPressedEastThisFrame[i] = Gamepad.all[i].buttonEast.wasPressedThisFrame;
            if (Gamepad.all[i].buttonEast.wasPressedThisFrame) { ExtendInputEastFrame(i); }
            else if (Gamepad.all[i].buttonSouth.wasPressedThisFrame) { ExtendInputSouthFrame(i); }
            else if (Gamepad.all[i].buttonWest.wasPressedThisFrame) { ExtendInputWestFrame(i); }
            else if (Gamepad.all[i].buttonNorth.wasPressedThisFrame) { ExtendInputNorthFrame(i); }
            isAnyButtonPressed[i] = isPressedEast[i] || isPressedSouth[i] || isPressedWest[i] || isPressedNorth[i];

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                leftStickValue[i].x = Gamepad.all[i].leftStick.ReadValue().x;
                leftStickValue[i].y = -Gamepad.all[i].leftStick.ReadValue().y;
            }
            else { leftStickValue[i] = Gamepad.all[i].leftStick.ReadValue(); }

            leftStickAbsValue[i].x = Mathf.Abs(leftStickValue[i].x);
            leftStickAbsValue[i].y = Mathf.Abs(leftStickValue[i].y);
        }
     }

    public bool InputEastThisFrame(int num)
    {
        bool returnValue = isPressedEastThisFrame[num];
        isPressedEastThisFrame[num] = false;
        isPressedEast[num] = false;
        return returnValue;
    }

    private void ExtendInputEastFrame(int num)
    {
        isPressedEast[num] = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedEast[num] = false; }));
    }

    private void ExtendInputSouthFrame(int num)
    {
        isPressedSouth[num] = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedSouth[num] = false; }));
    }

    private void ExtendInputWestFrame(int num)
    {
        isPressedWest[num] = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedWest[num] = false; }));
    }

    private void ExtendInputNorthFrame(int num)
    {
        isPressedNorth[num] = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedNorth[num] = false; }));
    }

    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++) { yield return null; }
        action();
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
