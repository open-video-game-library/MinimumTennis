using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Acceleration : MonoBehaviour, IResetParameter
{
    [SerializeField]
    private Players player;

    [SerializeField]
    private TMP_Text valueText;

    private Slider accelerationSlider;

    // Start is called before the first frame update
    void Start()
    {
        accelerationSlider = GetComponent<Slider>();
        accelerationSlider.value = Parameters.acceleration[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Acceleration" + ": " + Parameters.acceleration[(int)player].ToString("0.00");
    }

    public void ChangeParameter()
    {
        Parameters.acceleration[(int)player] = accelerationSlider.value;
        valueText.text = "P" + ((int)player + 1) + " Acceleration" + ": " + Parameters.acceleration[(int)player].ToString("0.00");
    }

    public void ResetParameter()
    {
        Parameters.acceleration[(int)player] = 1.0f;
        accelerationSlider.value = Parameters.acceleration[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Acceleration" + ": " + Parameters.acceleration[(int)player].ToString("0.00");
    }
}
