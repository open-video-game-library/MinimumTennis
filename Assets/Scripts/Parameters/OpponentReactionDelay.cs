using UnityEngine;
using UnityEngine.UI;

public class OpponentReactionDelay : MonoBehaviour
{
    [System.NonSerialized]
    public static int delay = 20;

    [SerializeField]
    private GameObject delayText;
    private Text valueText;

    private Slider opponentDelaySlider;

    // Start is called before the first frame update
    void Start()
    {
        opponentDelaySlider = GetComponent<Slider>();
        valueText = delayText.GetComponent<Text>();
        opponentDelaySlider.value = delay;
        valueText.text = "Opponent Reaction Delay" + ": " + delay;
    }

    public void ChangeDelay()
    {
        delay = (int)opponentDelaySlider.value;
        valueText.text = "Opponent Reaction Delay" + ": " + delay;
    }
}
