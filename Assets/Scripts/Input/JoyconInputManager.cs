using System;
using System.Collections.Generic;
using UnityEngine;

public enum SwingSide
{
    none,
    left,
    right
}

public enum Swing
{
    none,
    fore,
    back
}

public class JoyconInputManager : MonoBehaviour, IInputDevice
{
    // Editable Parameter
    private float threhold = 1.0f;

    // Joy-Con
    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon dominantJoycon;

    // スイング判定に使用するパラメータ
    private SwingSide swing;

    // スイング判定用の変数
    private readonly int defaultTestTime = 10;
    private float testTime;
    private bool tested;

    private int swingCoolTime = 60;

    private float maxAccel = 0.0f;
    private float minAccel = 0.0f;

    private Vector2 moveStickValue;

    private Swing swingResult;

    private bool isPressedEast;
    private bool isPressedSouth;
    private bool isPressedNorth;
    private bool isPressedWest;

    private bool isPressedStart;

    void Start()
    {
        threhold = Parameters.motionThrehold;

        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) { return; }

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        // 聞き手に応じてスイング判定に用いるJoy-Conを代入
        if (Parameters.charactersDominantHand[0] == DominantHand.left) { dominantJoycon = m_joyconL; }
        else { dominantJoycon = m_joyconR; }

        testTime = defaultTestTime;
    }

    void Update()
    {
        threhold = Parameters.motionThrehold;
    }

    void FixedUpdate()
    {
        // 左右のJoy-Conが繋がっていない場合
        if (m_joycons == null || m_joyconL == null || m_joyconR == null) { return; }

        if (m_joyconL != null)
        {
            float[] stick;

            if (Parameters.charactersDominantHand[0] == DominantHand.left) { stick = m_joyconR.GetStick(); }
            else { stick = m_joyconL.GetStick(); }

            moveStickValue = new Vector2(stick[0], stick[1]);
        }

        if (m_joyconR != null)
        {
            isPressedEast = dominantJoycon.GetButton(Joycon.Button.DPAD_RIGHT);
            isPressedSouth = dominantJoycon.GetButton(Joycon.Button.DPAD_DOWN);
            isPressedWest = dominantJoycon.GetButton(Joycon.Button.DPAD_LEFT);
            isPressedNorth = dominantJoycon.GetButton(Joycon.Button.DPAD_UP);

            isPressedStart = m_joyconR.GetButtonDown(Joycon.Button.PLUS) || m_joyconL.GetButtonDown(Joycon.Button.MINUS);
        }

        JudgeSwing();
    }

    // for Joy-Cons Debug
    private void OnGUI()
    {
        // Enterキーを押している間はJoy-Conのデバッグ情報を表示する
        if (!Input.GetKey(KeyCode.Return)) { return; }

        var style = GUI.skin.GetStyle("label");
        style.fontSize = 10;

        if (m_joycons == null || m_joycons.Count <= 0)
        {
            GUILayout.Label("Joy-Con is not connected.");
            return;
        }

        if (m_joyconL == null) { GUILayout.Label("Joy-Con (L) is not connected."); }
        else { GUILayout.Label("Joy-Con (L) is connected."); }

        if (m_joyconR == null) { GUILayout.Label("Joy-Con (R) is not connected."); }
        else { GUILayout.Label("Joy-Con (R) is connected."); }

        foreach (var joycon in m_joycons)
        {
            var isLeft = joycon.isLeft;
            var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
            var stick = joycon.GetStick();
            var gyro = joycon.GetGyro();
            var accel = joycon.GetAccel();
            var orientation = joycon.GetVector();

            GUILayout.BeginVertical(GUILayout.Width(480));
            GUILayout.Label(name);
            GUILayout.Label(string.Format("Stick：({0}, {1})", stick[0], stick[1]));
            GUILayout.Label("Gyro：" + gyro);
            GUILayout.Label("Acceleration：" + accel);
            GUILayout.Label("Orientation：" + orientation);

            GUILayout.EndVertical();
        }
    }

    private void JudgeSwing()
    {
        if (dominantJoycon.GetAccel().y * threhold > 1.0f && dominantJoycon.GetAccel().y * threhold < -2.50f
            && swing == SwingSide.none)
        {
            testTime = defaultTestTime;
            swing = SwingSide.left;
        }
        else if (dominantJoycon.GetAccel().y * threhold < -1.80f
            && swing == SwingSide.none)
        {
            testTime = defaultTestTime;
            swing = SwingSide.right;
        }

        if (swing != SwingSide.none)
        {
            if (swing == SwingSide.right)
            {
                if (testTime > 0)
                {
                    if (dominantJoycon.GetAccel().y * threhold > maxAccel) { maxAccel = dominantJoycon.GetAccel().y * threhold; }
                    tested = false;
                    testTime--;
                }
                else if (testTime <= 0 && !tested)
                {
                    if (Parameters.charactersDominantHand[0] == DominantHand.left) { swingResult = Swing.back; }
                    else { swingResult = Swing.fore; }

                    tested = true;
                }
            }
            else if (swing == SwingSide.left)
            {
                if (testTime > 0)
                {
                    if (dominantJoycon.GetAccel().y * threhold < minAccel) { minAccel = dominantJoycon.GetAccel().y * threhold; }
                    tested = false;
                    testTime--;
                }
                else if (testTime <= 0 && !tested)
                {
                    if (Parameters.charactersDominantHand[0] == DominantHand.left) { swingResult = Swing.fore; }
                    else { swingResult = Swing.back; }
                    
                    tested = true;
                }
            }

            swingCoolTime--;

            if (swingCoolTime < 0)
            {
                swing = SwingSide.none;
                swingResult = Swing.none;
                maxAccel = 0.0f;
                minAccel = 0.0f;
                testTime = defaultTestTime;
                swingCoolTime = 120;
            }
        }
    }

    public Vector2 GetMoveInput(Players player)
    {
        return moveStickValue;
    }

    public bool GetNormalShotInput(Players player)
    {
        return swingResult != Swing.none;
    }

    public bool GetLobShotInput(Players player)
    {
        return isPressedSouth && swingResult != Swing.none;
    }

    public bool GetFastShotInput(Players player)
    {
        return isPressedNorth && swingResult != Swing.none;
    }

    public bool GetDropShotInput(Players player)
    {
        return isPressedWest && swingResult != Swing.none;
    }

    public bool GetTossInput(Players player)
    {
        return isPressedEast;
    }

    public bool GetServeInput(Players player)
    {
        return swingResult != Swing.none;
    }

    public bool GetEscapeInput(Players player)
    {
        return isPressedStart;
    }
}
