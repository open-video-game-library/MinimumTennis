using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BallSpeed : MonoBehaviour, IResetParameter
{
    [SerializeField]
    private Players player;

    [SerializeField]
    private TMP_Text valueText;

    private Slider ballSpeedSlider;

    // Start is called before the first frame update
    void Start()
    {
        ballSpeedSlider = GetComponent<Slider>();
        ballSpeedSlider.value = Parameters.ballSpeed[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Ball Speed" + ": " + Parameters.ballSpeed[(int)player].ToString("0.00");
    }

    public void ChangeParameter()
    {
        Parameters.ballSpeed[(int)player] = ballSpeedSlider.value;
        valueText.text = "P" + ((int)player + 1) + " Ball Speed" + ": " + Parameters.ballSpeed[(int)player].ToString("0.00");
    }

    public void ResetParameter()
    {
        Parameters.ballSpeed[(int)player] = 1.0f;
        ballSpeedSlider.value = Parameters.ballSpeed[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Ball Speed" + ": " + Parameters.ballSpeed[(int)player].ToString("0.00");
    }
}
