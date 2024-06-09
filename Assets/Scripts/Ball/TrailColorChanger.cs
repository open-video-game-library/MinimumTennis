using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailColorChanger : MonoBehaviour
{
    private NormalBallController ballController;
    private TrailRenderer trailRenderer;

    private float ballSpeed;

    private Color trailColor = new Color(180, 255, 0, 20);

    // Start is called before the first frame update
    void Start()
    {
        ballController = GetComponent<NormalBallController>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailColor = new Color(1, 0, 0, 128);
    }

    // Update is called once per frame
    void Update()
    {
        ballSpeed = CalculateAbsolute(ballController.SpeedZ);
        trailColor = ConvertColor32(OptimizedSpeedForColor(ballSpeed), 1.0f, 1.0f); ;

        trailRenderer.material.color = trailColor;
    }

    private float CalculateAbsolute(float value)
    {
        float returnValue = value;

        if (returnValue < 0.0f) { returnValue *= -1.0f; }

        return returnValue;
    }

    private float OptimizedSpeedForColor(float speed)
    {
        float returnValue = speed;

        if (returnValue < 50.0f) { returnValue = 50.0f; }
        else if (returnValue > 100.0f) { returnValue = 100.0f; }

        returnValue = (-1.80f * returnValue + 190.0f) / 360.0f;

        return returnValue;
    }

    private Color32 ConvertColor32(float h, float s, float v)
    {
        Color32 returnValue;

        Color color = Color.HSVToRGB(h, s, v);
        returnValue = new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), 128);

        return returnValue;
    }
}
