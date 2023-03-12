using UnityEngine;
using UnityEngine.UI;

public class OpponentColor : MonoBehaviour
{
    [System.NonSerialized]
    public static Color32 opponentColor = new Color32(30, 30, 30, 255);

    [SerializeField]
    private GameObject opponent;

    [SerializeField]
    private GameObject opponentR;
    [SerializeField]
    private GameObject opponentG;
    [SerializeField]
    private GameObject opponentB;
    [SerializeField]
    private GameObject opponentA;

    private Slider opponentColorRSlider;
    private Slider opponentColorGSlider;
    private Slider opponentColorBSlider;
    private Slider opponentColorASlider;

    private Text opponentColorR;
    private Text opponentColorG;
    private Text opponentColorB;
    private Text opponentColorA;

    void Awake()
    {
        opponent.GetComponent<Renderer>().material.color = opponentColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        opponentColorRSlider = opponentR.GetComponent<Slider>();
        opponentColorGSlider = opponentG.GetComponent<Slider>();
        opponentColorBSlider = opponentB.GetComponent<Slider>();
        opponentColorASlider = opponentA.GetComponent<Slider>();

        opponentColorR = opponentR.GetComponentInChildren<Text>();
        opponentColorG = opponentG.GetComponentInChildren<Text>();
        opponentColorB = opponentB.GetComponentInChildren<Text>();
        opponentColorA = opponentA.GetComponentInChildren<Text>();

        opponentColorRSlider.value = opponentColor.r;
        opponentColorGSlider.value = opponentColor.g;
        opponentColorBSlider.value = opponentColor.b;
        opponentColorASlider.value = opponentColor.a;

        opponentColorR.text = "R：" + opponentColorRSlider.value;
        opponentColorG.text = "G：" + opponentColorGSlider.value;
        opponentColorB.text = "B：" + opponentColorBSlider.value;
        opponentColorA.text = "A：" + opponentColorASlider.value;
    }

    public void ChangeColorR()
    {
        opponentColorR.text = "R：" + opponentColorRSlider.value;
        opponentColor.r = (byte)opponentColorRSlider.value;
        opponent.GetComponent<Renderer>().material.color = opponentColor;
    }

    public void ChangeColorG()
    {
        opponentColorG.text = "G：" + opponentColorGSlider.value;
        opponentColor.g = (byte)opponentColorGSlider.value;
        opponent.GetComponent<Renderer>().material.color = opponentColor;
    }

    public void ChangeColorB()
    {
        opponentColorB.text = "B：" + opponentColorBSlider.value;
        opponentColor.b = (byte)opponentColorBSlider.value;
        opponent.GetComponent<Renderer>().material.color = opponentColor;
    }

    public void ChangeColorA()
    {
        opponentColorA.text = "A：" + opponentColorASlider.value;
        opponentColor.a = (byte)opponentColorASlider.value;
        opponent.GetComponent<Renderer>().material.color = opponentColor;
    }
}
