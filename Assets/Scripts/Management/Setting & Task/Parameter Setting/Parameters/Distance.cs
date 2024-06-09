using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Distance : MonoBehaviour, IResetParameter
{
    [SerializeField]
    private Players player;

    [SerializeField]
    private TMP_Text valueText;

    private Slider distanceSlider;

    // Start is called before the first frame update
    void Start()
    {
        distanceSlider = GetComponent<Slider>();
        distanceSlider.value = Parameters.distance[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Distance" + ": " + Parameters.distance[(int)player].ToString("0.00");
    }

    public void ChangeParameter()
    {
        Parameters.distance[(int)player] = distanceSlider.value;
        valueText.text = "P" + ((int)player + 1) + " Distance" + ": " + Parameters.distance[(int)player].ToString("0.00");
    }

    public void ResetParameter()
    {
        Parameters.distance[(int)player] = 0.50f;
        distanceSlider.value = Parameters.distance[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Distance" + ": " + Parameters.distance[(int)player].ToString("0.00");
    }
}
