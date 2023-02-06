using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class JoyconInputManager : MonoBehaviour
{
    private float threhold = 1.0f;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    [NonSerialized]
    public string swing = null;
    private readonly int defaultTestTime = 10;
    private int testTime;
    private bool tested;
    private int swingCoolTime = 60;

    private float maxAccel = 0.0f;
    private float minAccel = 0.0f;

    [NonSerialized]
    public bool fore;
    [NonSerialized]
    public bool back;
    [NonSerialized]
    public bool anySwing;
    [NonSerialized]
    public bool tos;

    [NonSerialized]
    public bool isPressedEast;
    [NonSerialized]
    public bool isPressedSouth;
    [NonSerialized]
    public bool isPressedNorth;
    [NonSerialized]
    public bool isPressedWest;

    void Start()
    {
        testTime = defaultTestTime;

        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) { return; }

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        threhold = MotionThrehold.motionThrehold;
    }

    void Update()
    {
        threhold = MotionThrehold.motionThrehold;
        
        if (m_joyconR != null || m_joyconL != null)
        {
            if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)) { tos = true; }
            else { tos = false; }

            isPressedEast = m_joyconR.GetButton(Joycon.Button.DPAD_RIGHT);
            isPressedSouth = m_joyconR.GetButton(Joycon.Button.DPAD_DOWN);
            isPressedWest = m_joyconR.GetButton(Joycon.Button.DPAD_LEFT);
            isPressedNorth = m_joyconR.GetButton(Joycon.Button.DPAD_UP);
        }

        anySwing = fore || back;
    }

    void FixedUpdate()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count < 1) return;

        foreach (var button in m_buttons)
        {
            if (m_joyconL != null && m_joyconL.GetButton(button)) { m_pressedButtonL = button; }
            if (m_joyconR != null && m_joyconR.GetButton(button)) { m_pressedButtonR = button; }
        }

        if (m_joyconR.GetAccel().y * threhold > 1.0f && m_joyconR.GetAccel().y * threhold < -2.50f
            && swing == null)
        {
            testTime = defaultTestTime;
            swing = "back";
        }
        else if (m_joyconR.GetAccel().y * threhold < -1.80f
            && swing == null)
        {
            testTime = defaultTestTime;
            swing = "fore";
        }

        if (swing != null)
        {
            if (swing == "fore")
            {
                if (testTime > 0)
                {
                    if (m_joyconR.GetAccel().y * threhold > maxAccel) { maxAccel = m_joyconR.GetAccel().y * threhold; }
                    tested = false;
                    testTime--;
                }
                else if (testTime <= 0 && !tested)
                {
                    fore = true;
                    tested = true;
                }
            }
            else if (swing == "back")
            {
                if (testTime > 0)
                {
                    if (m_joyconR.GetAccel().y * threhold < minAccel) { minAccel = m_joyconR.GetAccel().y * threhold; }
                    tested = false;
                    testTime--;
                }
                else if (testTime <= 0 && !tested)
                {
                    back = true;
                    tested = true;
                }
            }

            swingCoolTime--;

            if (swingCoolTime < 0)
            {
                swing = null;
                fore = false;
                back = false;
                maxAccel = 0.0f;
                minAccel = 0.0f;
                testTime = defaultTestTime;
                swingCoolTime = 120;
            }
        }
    }

    private void OnGUI()
    {
        // Display Joy-Con sensor information while pressing Enter key
        if (!Input.GetKey(KeyCode.Return)) { return; }

        var style = GUI.skin.GetStyle("label");
        style.fontSize = 20;

        if (m_joycons == null || m_joycons.Count <= 0)
        {
            GUILayout.Label("Joy-Con is not connected.");
            return;
        }

        if (m_joyconL == null) { GUILayout.Label("Joy-Con (L) is not connected."); }
        if (m_joyconR == null)
        {
            GUILayout.Label("Joy-Con (R) is not connected.");
            return;
        }

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        // Read only right side of Joy-Con (for debugging)
        var name = "Joy-Con (R)";
        var button = m_pressedButtonR;
        var stick = m_joyconR.GetStick();
        var gyro = m_joyconR.GetGyro();
        var accel = m_joyconR.GetAccel();
        var orientation = m_joyconR.GetVector();

        GUILayout.BeginVertical(GUILayout.Width(480));
        GUILayout.Label(name);
        GUILayout.Label("Pressed Button：" + button);
        GUILayout.Label(string.Format("Stick：({0}, {1})", stick[0], stick[1]));
        GUILayout.Label("Gyro：" + gyro);
        GUILayout.Label("Acceleration：" + accel);
        GUILayout.Label("Orientation：" + orientation);
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }
}
