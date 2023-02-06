using UnityEngine;
using UnityEngine.UI;

public class MotionThrehold : MonoBehaviour
{
    [System.NonSerialized]
    public static float motionThrehold = 1.0f;

    [SerializeField]
    private GameObject motionThreholdText;
    private Text valueText;

    private Slider motionThreholdSlider;

    // Start is called before the first frame update
    void Start()
    {
        motionThreholdSlider = GetComponent<Slider>();
        valueText = motionThreholdText.GetComponent<Text>();
        motionThreholdSlider.value = motionThrehold;
        valueText.text = "Motion Threhold" + ": " + motionThrehold.ToString("0.0");
    }

    public void ChangeMotionThrehold()
    {
        motionThrehold = motionThreholdSlider.value;
        valueText.text = "Motion Threhold" + ": " + motionThrehold.ToString("0.0");
    }
}
