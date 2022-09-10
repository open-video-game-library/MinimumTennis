using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    public GameObject player;

    public GameObject playerR;
    public GameObject playerG;
    public GameObject playerB;
    public GameObject playerA;

    private Slider playerColorRSlider;
    private Slider playerColorGSlider;
    private Slider playerColorBSlider;
    private Slider playerColorASlider;

    private Text playerColorR;
    private Text playerColorG;
    private Text playerColorB;
    private Text playerColorA;

    private Color32 playerColor;

    // Start is called before the first frame update
    void Start()
    {
        playerColorRSlider = playerR.GetComponent<Slider>();
        playerColorGSlider = playerG.GetComponent<Slider>();
        playerColorBSlider = playerB.GetComponent<Slider>();
        playerColorASlider = playerA.GetComponent<Slider>();

        playerColorR = playerR.GetComponentInChildren<Text>();
        playerColorG = playerG.GetComponentInChildren<Text>();
        playerColorB = playerB.GetComponentInChildren<Text>();
        playerColorA = playerA.GetComponentInChildren<Text>();

        playerColor = player.GetComponent<Renderer>().material.color;

        playerColorRSlider.value = playerColor.r;
        playerColorGSlider.value = playerColor.g;
        playerColorBSlider.value = playerColor.b;
        playerColorASlider.value = playerColor.a;

        playerColorR.text = "R：" + playerColorRSlider.value;
        playerColorG.text = "G：" + playerColorGSlider.value;
        playerColorB.text = "B：" + playerColorBSlider.value;
        playerColorA.text = "A：" + playerColorASlider.value;
    }

    public void ChangeColorR()
    {
        playerColorR.text = "R：" + playerColorRSlider.value;
        playerColor.r = (byte)playerColorRSlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }

    public void ChangeColorG()
    {
        playerColorG.text = "G：" + playerColorGSlider.value;
        playerColor.g = (byte)playerColorGSlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }

    public void ChangeColorB()
    {
        playerColorB.text = "B：" + playerColorBSlider.value;
        playerColor.b = (byte)playerColorBSlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }

    public void ChangeColorA()
    {
        playerColorA.text = "A：" + playerColorASlider.value;
        playerColor.a = (byte)playerColorASlider.value;
        player.GetComponent<Renderer>().material.color = playerColor;
    }
}
