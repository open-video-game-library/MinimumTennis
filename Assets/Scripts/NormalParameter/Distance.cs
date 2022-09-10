using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour
{
    private Slider distanceSlider;

    public GameObject distanceText;
    private Text distance;

    // Start is called before the first frame update
    void Start()
    {
        distanceSlider = GetComponent<Slider>();
        distance = distanceText.GetComponent<Text>();
        distanceSlider.value = EnemyController.distance;
        distance.text = "Distance" + ": " + EnemyController.distance.ToString("0.00");
    }

    public void ChangeDistance()
    {
        EnemyController.distance = distanceSlider.value;
        distance.text = "Distance" + ": " + EnemyController.distance.ToString("0.00");
    }
}
