using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    [System.NonSerialized]
    public static Color32 playerColor = new Color32(210, 210, 210, 255);

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject playerR;
    [SerializeField]
    private GameObject playerG;
    [SerializeField]
    private GameObject playerB;
    [SerializeField]
    private GameObject playerA;

    private Slider playerColorRSlider;
    private Slider playerColorGSlider;
    private Slider playerColorBSlider;
    private Slider playerColorASlider;

    private Text playerColorRText;
    private Text playerColorGText;
    private Text playerColorBText;
    private Text playerColorAText;

    void Awake()
    {
        player.GetComponent<Renderer>().material.color = playerColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerColorRSlider = playerR.GetComponent<Slider>();
        playerColorGSlider = playerG.GetComponent<Slider>();
        playerColorBSlider = playerB.GetComponent<Slider>();
        playerColorASlider = playerA.GetComponent<Slider>();

        playerColorRText = playerR.GetComponentInChildren<Text>();
        playerColorGText = playerG.GetComponentInChildren<Text>();
        playerColorBText = playerB.GetComponentInChildren<Text>();
        playerColorAText = playerA.GetComponentInChildren<Text>();

        playerColorRSlider.value = playerColor.r;
        playerColorGSlider.value = playerColor.g;
        playerColorBSlider.value = playerColor.b;
        playerColorASlider.value = playerColor.a;

        playerColorRText.text = "R：" + playerColorRSlider.value;
        playerColorGText.text = "G：" + playerColorGSlider.value;
        playerColorBText.text = "B：" + playerColorBSlider.value;
        playerColorAText.text = "A：" + playerColorASlider.value;
    }

    public void ChangeColorR()
    {
        playerColorRText.text = "R：" + playerColorRSlider.value;
        playerColor.r = (byte)playerColorRSlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }

    public void ChangeColorG()
    {
        playerColorGText.text = "G：" + playerColorGSlider.value;
        playerColor.g = (byte)playerColorGSlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }

    public void ChangeColorB()
    {
        playerColorBText.text = "B：" + playerColorBSlider.value;
        playerColor.b = (byte)playerColorBSlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }

    public void ChangeColorA()
    {
        playerColorAText.text = "A：" + playerColorASlider.value;
        playerColor.a = (byte)playerColorASlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }
}
