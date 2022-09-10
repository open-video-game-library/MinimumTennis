using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyReactionDelay : MonoBehaviour
{
    private Slider enemyDelaySlider;

    public GameObject delayText;
    private Text enemyDelay;

    // Start is called before the first frame update
    void Start()
    {
        enemyDelaySlider = GetComponent<Slider>();
        enemyDelay = delayText.GetComponent<Text>();
        enemyDelaySlider.value = PlayerController.delay;
        enemyDelay.text = "Opponent Reaction Delay" + ": " + PlayerController.delay;
    }

    public void ChangeDelay()
    {
        PlayerController.delay = (int)enemyDelaySlider.value;
        enemyDelay.text = "Opponent Reaction Delay" + ": " + PlayerController.delay;
    }
}
