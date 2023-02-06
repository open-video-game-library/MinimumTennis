using UnityEngine;
using UnityEngine.UI;

public class PlayerAcceleration : MonoBehaviour
{
    [System.NonSerialized]
    public static float playerAcceleration = 1.0f;

    [SerializeField]
    private GameObject playerAccelerationText;
    private Text valueText;

    private Slider playerAccelerationSlider;

    // Start is called before the first frame update
    void Start()
    {
        playerAccelerationSlider = GetComponent<Slider>();
        valueText = playerAccelerationText.GetComponent<Text>();
        playerAccelerationSlider.value = playerAcceleration;
        valueText.text = "Player Acceleration" + ": " + playerAcceleration.ToString("0.00");
    }

    public void ChangeAcceleration()
    {
        playerAcceleration = playerAccelerationSlider.value;
        valueText.text = "Player Acceleration" + ": " + playerAcceleration.ToString("0.00");
    }
}
