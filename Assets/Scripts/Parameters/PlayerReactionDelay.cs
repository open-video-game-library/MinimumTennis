using UnityEngine;
using UnityEngine.UI;

public class PlayerReactionDelay : MonoBehaviour
{
    [System.NonSerialized]
    public static int delay = 20;

    [SerializeField]
    private GameObject delayText;
    private Text valueText;

    private Slider playerDelaySlider;

    // Start is called before the first frame update
    void Start()
    {
        playerDelaySlider = GetComponent<Slider>();
        valueText = delayText.GetComponent<Text>();
        playerDelaySlider.value = delay;
        valueText.text = "Player Reaction Delay" + ": " + delay;
    }

    public void ChangeDelay()
    {
        delay = (int)playerDelaySlider.value;
        valueText.text = "Player Reaction Delay" + ": " + delay;
    }
}
