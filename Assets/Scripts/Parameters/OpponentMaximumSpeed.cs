using UnityEngine;
using UnityEngine.UI;

public class OpponentMaximumSpeed : MonoBehaviour
{
    [System.NonSerialized]
    public static float opponentMaximumSpeed = 30.0f;

    [SerializeField]
    private GameObject OpponentMaximumSpeedText;
    private Text valueText;

    private Slider opponentMaximumSpeedSlider;

    [SerializeField]
    private GameObject opponentAccelerationSlider;
    private Slider accelerationSlider;

    // Start is called before the first frame update
    void Start()
    {
        opponentMaximumSpeedSlider = GetComponent<Slider>();
        valueText = OpponentMaximumSpeedText.GetComponent<Text>();
        opponentMaximumSpeedSlider.value = opponentMaximumSpeed;
        valueText.text = "Opponent Maximum Speed" + ": " + (opponentMaximumSpeed).ToString("0.00");

        accelerationSlider = opponentAccelerationSlider.GetComponent<Slider>();
        accelerationSlider.maxValue = opponentMaximumSpeed;
    }

    public void ChangeMaximumSpeed()
    {
        opponentMaximumSpeed = opponentMaximumSpeedSlider.value;
        valueText.text = "Opponent Maximum Speed" + ": " + (opponentMaximumSpeed).ToString("0.00");

        if (accelerationSlider) { accelerationSlider.maxValue = opponentMaximumSpeed; }
    }
}
