using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Threhold : MonoBehaviour
{
    private Slider motionSlider;

    public GameObject motionText;
    private Text threhold;

    // Start is called before the first frame update
    void Start()
    {
        motionSlider = GetComponent<Slider>();
        threhold = motionText.GetComponent<Text>();
        motionSlider.value = JoyConManager.threhold;
        threhold.text = "Motion Threhold" + ": " + JoyConManager.threhold.ToString("0.00");
    }

    public void ChangeThrehold()
    {
        JoyConManager.threhold = motionSlider.value;
        threhold.text = "Motion Threhold" + ": " + JoyConManager.threhold.ToString("0.00");
    }
}
