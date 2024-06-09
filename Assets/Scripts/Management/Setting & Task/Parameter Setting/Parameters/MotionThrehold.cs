using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MotionThrehold : MonoBehaviour, IResetParameter
{
    [SerializeField]
    private TMP_Text valueText;

    private Slider motionThreholdSlider;

    // Start is called before the first frame update
    void Start()
    {
        motionThreholdSlider = GetComponent<Slider>();
        motionThreholdSlider.value = Parameters.motionThrehold;
        valueText.text = "Motion Threhold (Motion)" + ": " + Parameters.motionThrehold.ToString("0.0");
    }

    public void ChangeParameter()
    {
        Parameters.motionThrehold = motionThreholdSlider.value;
        valueText.text = "Motion Threhold (Motion)" + ": " + Parameters.motionThrehold.ToString("0.0");
    }

    public void ResetParameter()
    {
        Parameters.motionThrehold = 1.0f;
        motionThreholdSlider.value = Parameters.motionThrehold;
        valueText.text = "Motion Threhold (Motion)" + ": " + Parameters.motionThrehold.ToString("0.0");
    }
}
