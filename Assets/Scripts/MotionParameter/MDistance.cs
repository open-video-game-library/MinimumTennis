using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MDistance : MonoBehaviour
{
    private Slider distanceSlider;

    public GameObject distanceText;
    private Text distance;

    // Start is called before the first frame update
    void Start()
    {
        distanceSlider = GetComponent<Slider>();
        distance = distanceText.GetComponent<Text>();
        distanceSlider.value = MEnemyController.distance;
        distance.text = "Distance" + ": " + MEnemyController.distance.ToString("0.00");
    }

    public void ChangeDistance()
    {
        MEnemyController.distance = distanceSlider.value;
        distance.text = "Distance" + ": " + MEnemyController.distance.ToString("0.00");
    }
}
