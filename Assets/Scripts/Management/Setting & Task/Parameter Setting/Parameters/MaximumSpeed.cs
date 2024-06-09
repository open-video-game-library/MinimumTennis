using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MaximumSpeed : MonoBehaviour, IResetParameter
{
    [SerializeField]
    private Players player;

    [SerializeField] 
    private TMP_Text valueText;

    private Slider maximumSpeedSlider;

    [SerializeField]
    private GameObject accelerationSliderObject;
    private Slider accelerationSlider;

    // Start is called before the first frame update
    void Start()
    {
        maximumSpeedSlider = GetComponent<Slider>();
        maximumSpeedSlider.value = Parameters.maximumSpeed[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Maximum Speed" + ": " + Parameters.maximumSpeed[(int)player].ToString("0.00");

        accelerationSlider = accelerationSliderObject.GetComponent<Slider>();
        accelerationSlider.maxValue = Parameters.maximumSpeed[(int)player];
    }

    public void ChangeParameter()
    {
        Parameters.maximumSpeed[(int)player] = maximumSpeedSlider.value;
        valueText.text = "P" + ((int)player + 1) + " Maximum Speed" + ": " + Parameters.maximumSpeed[(int)player].ToString("0.00");

        if (accelerationSlider) { accelerationSlider.maxValue = Parameters.maximumSpeed[(int)player]; }
    }

    public void ResetParameter()
    {
        Parameters.maximumSpeed[(int)player] = 30.0f;
        maximumSpeedSlider.value = Parameters.maximumSpeed[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Maximum Speed" + ": " + Parameters.maximumSpeed[(int)player].ToString("0.00");

        if (accelerationSlider) { accelerationSlider.maxValue = Parameters.maximumSpeed[(int)player]; }
    }
}
