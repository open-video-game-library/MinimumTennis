using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPlayerReactionDelay : MonoBehaviour
{
    private Slider playerDelaySlider;

    public GameObject delayText;
    private Text playerDelay;

    // Start is called before the first frame update
    void Start()
    {
        playerDelaySlider = GetComponent<Slider>();
        playerDelay = delayText.GetComponent<Text>();
        playerDelaySlider.value = MEnemyController.delay;
        playerDelay.text = "Player Reaction Delay" + ": " + MEnemyController.delay;
    }

    public void ChangeDelay()
    {
        MEnemyController.delay = (int)playerDelaySlider.value;
        playerDelay.text = "Player Reaction Delay" + ": " + MEnemyController.delay;
    }
}
