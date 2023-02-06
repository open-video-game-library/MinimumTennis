using System.Collections;
using UnityEngine;
using System;

public class KeyboardInputManager : MonoBehaviour
{
    [NonSerialized]
    public bool isPressedA = false;
    [NonSerialized]
    public bool isPressedD = false;
    [NonSerialized]
    public bool isPressedW = false;
    [NonSerialized]
    public bool isPressedS = false;
    [NonSerialized]
    public bool isPressedL = false;
    [NonSerialized]
    public bool isPressedK = false;
    [NonSerialized]
    public bool isPressedI = false;
    [NonSerialized]
    public bool isPressedJ = false;
    [NonSerialized]
    public bool isAnyKeyPressed = false;

    private readonly int defaultExtensionFrame = 30;

    void Update()
    {
        isPressedA = Input.GetKey(KeyCode.A);
        isPressedD = Input.GetKey(KeyCode.D);
        isPressedW = Input.GetKey(KeyCode.W);
        isPressedS = Input.GetKey(KeyCode.S);

        // Only once per frame to determine if the button is pressed.
        if (Input.GetKeyDown(KeyCode.L)) { ExtendInputFrameL(); }
        else if (Input.GetKeyDown(KeyCode.K)) { ExtendInputFrameK(); }
        else if (Input.GetKeyDown(KeyCode.I)) { ExtendInputFrameI(); }
        else if (Input.GetKeyDown(KeyCode.J)) { ExtendInputFrameJ(); }
        isAnyKeyPressed = isPressedL || isPressedK || isPressedI || isPressedJ;
    }

    private void ExtendInputFrameL()
    {
        isPressedL = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedL = false; }));
    }

    private void ExtendInputFrameK()
    {
        isPressedK = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedK = false; }));
    }

    private void ExtendInputFrameI()
    {
        isPressedI = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedI = false; }));
    }

    private void ExtendInputFrameJ()
    {
        isPressedJ = true;
        StartCoroutine(DelayMethod(defaultExtensionFrame, () => { isPressedJ = false; }));
    }

    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++) { yield return null; }
        action();
    }
}
