using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEnemyReactionDelay : MonoBehaviour
{
    private Slider enemyDelaySlider;

    public GameObject delayText;
    private Text enemyDelay;

    // Start is called before the first frame update
    void Start()
    {
        enemyDelaySlider = GetComponent<Slider>();
        enemyDelay = delayText.GetComponent<Text>();
        enemyDelaySlider.value = MPlayerController.delay;
        enemyDelay.text = "Opponent Reaction Delay" + ": " + MPlayerController.delay;
    }

    public void ChangeDelay()
    {
        MPlayerController.delay = (int)enemyDelaySlider.value;
        enemyDelay.text = "Opponent Reaction Delay" + ": " + MPlayerController.delay;
    }
}
