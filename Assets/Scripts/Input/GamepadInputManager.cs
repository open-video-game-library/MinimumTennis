using System.Collections;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GamepadInputManager : MonoBehaviour, IInputDevice
{
    private readonly int playerNum = 2;

    private bool[] isConnected;

    private Vector2[] leftStickValue;
    
    private bool[] isPressedEast;
    private bool[] isPressedSouth;
    private bool[] isPressedWest;
    private bool[] isPressedNorth;

    private bool[] isPressedServe;
    private bool[] isPressedStart;

    // ショット用のボタンを押したあと、ボタンを押した判定が指定フレーム数持続する
    private readonly int extensionFrame = 30;

    // Start is called before the first frame update
    void Start()
    {
        isConnected = new bool[playerNum];

        leftStickValue = new Vector2[playerNum];
        
        isPressedEast = new bool[playerNum];
        isPressedSouth = new bool[playerNum];
        isPressedWest = new bool[playerNum];
        isPressedNorth = new bool[playerNum];

        isPressedServe = new bool[playerNum];
        isPressedStart = new bool[playerNum];
    }

    void FixedUpdate()
    {
        int gamepadNum;
        if (Gamepad.all.Count < playerNum) { gamepadNum = Gamepad.all.Count; }
        else { gamepadNum = playerNum; }

        for (int i = 0; i < gamepadNum; i++)
        {
            isConnected[i] = Gamepad.all[i] != null;

            leftStickValue[i] = Gamepad.all[i].leftStick.ReadValue();

            if (Gamepad.all[i].buttonEast.wasPressedThisFrame) { ExtendInputEastFrame(i); }
            else if (Gamepad.all[i].buttonSouth.wasPressedThisFrame) { ExtendInputSouthFrame(i); }
            else if (Gamepad.all[i].buttonNorth.wasPressedThisFrame) { ExtendInputNorthFrame(i); }
            else if (Gamepad.all[i].buttonWest.wasPressedThisFrame) { ExtendInputWestFrame(i); }

            isPressedServe[i] = Gamepad.all[i].buttonEast.wasPressedThisFrame;
            isPressedStart[i] = Gamepad.all[i].startButton.wasPressedThisFrame;
        }
    }

    public Vector2 GetMoveInput(Players player)
    {
        return leftStickValue[(int)player];
    }

    public bool GetNormalShotInput(Players player)
    {
        return isPressedEast[(int)player];
    }

    public bool GetLobShotInput(Players player)
    {
        return isPressedSouth[(int)player];
    }

    public bool GetFastShotInput(Players player)
    {
        return isPressedNorth[(int)player];
    }

    public bool GetDropShotInput(Players player)
    {
        return isPressedWest[(int)player];
    }

    public bool GetTossInput(Players player)
    {
        return isPressedServe[(int)player];
    }

    public bool GetServeInput(Players player)
    {
        return isPressedServe[(int)player];
    }

    public bool GetEscapeInput(Players player)
    {
        return isPressedStart[(int)player];
    }

    private void ExtendInputEastFrame(int playNum)
    {
        isPressedEast[playNum] = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedEast[playNum] = false; }));
    }

    private void ExtendInputSouthFrame(int playNum)
    {
        isPressedSouth[playNum] = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedSouth[playNum] = false; }));
    }

    private void ExtendInputNorthFrame(int playNum)
    {
        isPressedNorth[playNum] = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedNorth[playNum] = false; }));
    }

    private void ExtendInputWestFrame(int playNum)
    {
        isPressedWest[playNum] = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedWest[playNum] = false; }));
    }

    private IEnumerator ExtendFrame(int frame, Action action)
    {
        for (int i = 0; i < frame; i++) { yield return null; }
        action();
    }
}
