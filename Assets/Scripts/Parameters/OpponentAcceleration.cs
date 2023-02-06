using UnityEngine;
using UnityEngine.UI;

public class OpponentAcceleration : MonoBehaviour
{
    [System.NonSerialized]
    public static float opponentAcceleration = 1.0f;

    [SerializeField]
    private GameObject opponentAccelerationText;
    private Text valueText;

    private Slider opponentAccelerationSlider;

    // Start is called before the first frame update
    void Start()
    {
        opponentAccelerationSlider = GetComponent<Slider>();
        valueText = opponentAccelerationText.GetComponent<Text>();
        opponentAccelerationSlider.value = opponentAcceleration;
        valueText.text = "Opponent Acceleration" + ": " + opponentAcceleration.ToString("0.00");
    }

    public void ChangeAcceleration()
    {
        opponentAcceleration = opponentAccelerationSlider.value;
        valueText.text = "Oppponent Acceleration" + ": " + opponentAcceleration.ToString("0.00");
    }
}
