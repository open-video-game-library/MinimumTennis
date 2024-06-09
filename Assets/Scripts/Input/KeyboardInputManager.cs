using System.Collections;
using UnityEngine;
using System;

public class KeyboardInputManager : MonoBehaviour, IInputDevice
{
    private bool isPressedA;
    private bool isPressedD;
    private bool isPressedW;
    private bool isPressedS;

    private bool isPressedL;
    private bool isPressedK;
    private bool isPressedI;
    private bool isPressedJ;

    private bool isPressedSpace;
    private bool isPressedEscape;

    private readonly int extensionFrame = 30;

    void FixedUpdate()
    {
        isPressedA = Input.GetKey(KeyCode.A);
        isPressedD = Input.GetKey(KeyCode.D);
        isPressedW = Input.GetKey(KeyCode.W);
        isPressedS = Input.GetKey(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.L)) { ExtendInputEastFrame(); }
        else if (Input.GetKeyDown(KeyCode.K)) { ExtendInputSouthFrame(); }
        else if (Input.GetKeyDown(KeyCode.I)) { ExtendInputNorthFrame(); }
        else if (Input.GetKeyDown(KeyCode.J)) { ExtendInputWestFrame(); }

        isPressedSpace = Input.GetKeyDown(KeyCode.Space);
        isPressedEscape = Input.GetKeyDown(KeyCode.Escape);
    }

    public Vector2 GetMoveInput(Players player)
    {
        float leftValue = isPressedA ? -1.0f : 0.0f;
        float rightValue = isPressedD ? 1.0f : 0.0f;
        float frontValue = isPressedW ? 1.0f : 0.0f;
        float backValue = isPressedS ? -1.0f : 0.0f;

        return new Vector2(leftValue + rightValue, frontValue + backValue);
    }

    public bool GetNormalShotInput(Players player)
    {
        return isPressedL;
    }

    public bool GetLobShotInput(Players player)
    {
        return isPressedK;
    }

    public bool GetFastShotInput(Players player)
    {
        return isPressedI;
    }

    public bool GetDropShotInput(Players player)
    {
        return isPressedJ;
    }

    public bool GetTossInput(Players player)
    {
        return isPressedSpace;
    }

    public bool GetServeInput(Players player)
    {
        return isPressedSpace;
    }

    public bool GetEscapeInput(Players player)
    {
        return isPressedEscape;
    }

    private void ExtendInputEastFrame()
    {
        isPressedL = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedL = false; }));
    }

    private void ExtendInputSouthFrame()
    {
        isPressedK = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedK = false; }));
    }

    private void ExtendInputNorthFrame()
    {
        isPressedI = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedI = false; }));
    }

    private void ExtendInputWestFrame()
    {
        isPressedJ = true;
        StartCoroutine(ExtendFrame(extensionFrame, () => { isPressedJ = false; }));
    }

    private IEnumerator ExtendFrame(int frame, Action action)
    {
        for (int i = 0; i < frame; i++) { yield return null; }
        action();
    }
}
