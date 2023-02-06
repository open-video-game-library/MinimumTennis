using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour
{
    [System.NonSerialized]
    public static float distance = 0.50f;

    [SerializeField]
    private GameObject distanceText;
    private Text valueText;

    private Slider distanceSlider;

    // Start is called before the first frame update
    void Start()
    {
        distanceSlider = GetComponent<Slider>();
        valueText = distanceText.GetComponent<Text>();
        distanceSlider.value = distance;
        valueText.text = "Distance" + ": " + distance.ToString("0.00");
    }

    public void ChangeDistance()
    {
        distance = distanceSlider.value;
        valueText.text = "Distance" + ": " + distance.ToString("0.00");
    }
}
