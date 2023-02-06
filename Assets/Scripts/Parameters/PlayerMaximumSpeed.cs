using UnityEngine;
using UnityEngine.UI;

public class PlayerMaximumSpeed : MonoBehaviour
{
    [System.NonSerialized]
    public static float playerMaximumSpeed = 30.0f;

    [SerializeField] 
    private GameObject playerMaximumSpeedText;
    private Text valueText;

    private Slider playerMaximumSpeedSlider;

    [SerializeField]
    private GameObject playerAccelerationSlider;
    private Slider accelerationSlider;

    // Start is called before the first frame update
    void Start()
    {
        playerMaximumSpeedSlider = GetComponent<Slider>();
        valueText = playerMaximumSpeedText.GetComponent<Text>();
        playerMaximumSpeedSlider.value = playerMaximumSpeed;
        valueText.text = "Player Maximum Speed" + ": " + (playerMaximumSpeed).ToString("0.00");

        accelerationSlider = playerAccelerationSlider.GetComponent<Slider>();
        accelerationSlider.maxValue = playerMaximumSpeed;
    }

    public void ChangeMaximumSpeed()
    {
        playerMaximumSpeed = playerMaximumSpeedSlider.value;
        valueText.text = "Player Maximum Speed" + ": " + (playerMaximumSpeed).ToString("0.00");

        if (accelerationSlider) { accelerationSlider.maxValue = playerMaximumSpeed; }
    }
}
