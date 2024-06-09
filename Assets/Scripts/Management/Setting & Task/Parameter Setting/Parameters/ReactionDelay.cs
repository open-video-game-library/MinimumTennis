using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReactionDelay : MonoBehaviour, IResetParameter
{
    [SerializeField]
    private Players player;

    [SerializeField]
    private TMP_Text valueText;

    private Slider delaySlider;

    // Start is called before the first frame update
    void Start()
    {
        delaySlider = GetComponent<Slider>();
        delaySlider.value = Parameters.reactionDelay[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Reaction Delay" + ": " + Parameters.reactionDelay[(int)player].ToString("0.00");
    }

    public void ChangeParameter()
    {
        Parameters.reactionDelay[(int)player] = delaySlider.value;
        valueText.text = "P" + ((int)player + 1) + " Reaction Delay" + ": " + Parameters.reactionDelay[(int)player].ToString("0.00");
    }

    public void ResetParameter()
    {
        Parameters.reactionDelay[(int)player] = 0.30f;
        delaySlider.value = Parameters.reactionDelay[(int)player];
        valueText.text = "P" + ((int)player + 1) + " Reaction Delay" + ": " + Parameters.reactionDelay[(int)player].ToString("0.00");
    }
}
