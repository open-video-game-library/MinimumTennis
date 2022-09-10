using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyColor : MonoBehaviour
{
    public GameObject enemy;

    public GameObject enemyR;
    public GameObject enemyG;
    public GameObject enemyB;
    public GameObject enemyA;

    private Slider enemyColorRSlider;
    private Slider enemyColorGSlider;
    private Slider enemyColorBSlider;
    private Slider enemyColorASlider;

    private Text enemyColorR;
    private Text enemyColorG;
    private Text enemyColorB;
    private Text enemyColorA;

    private Color32 enemyColor;

    // Start is called before the first frame update
    void Start()
    {
        enemyColorRSlider = enemyR.GetComponent<Slider>();
        enemyColorGSlider = enemyG.GetComponent<Slider>();
        enemyColorBSlider = enemyB.GetComponent<Slider>();
        enemyColorASlider = enemyA.GetComponent<Slider>();

        enemyColorR = enemyR.GetComponentInChildren<Text>();
        enemyColorG = enemyG.GetComponentInChildren<Text>();
        enemyColorB = enemyB.GetComponentInChildren<Text>();
        enemyColorA = enemyA.GetComponentInChildren<Text>();

        enemyColor = enemy.GetComponent<Renderer>().material.color;

        enemyColorRSlider.value = enemyColor.r;
        enemyColorGSlider.value = enemyColor.g;
        enemyColorBSlider.value = enemyColor.b;
        enemyColorASlider.value = enemyColor.a;

        enemyColorR.text = "R：" + enemyColorRSlider.value;
        enemyColorG.text = "G：" + enemyColorGSlider.value;
        enemyColorB.text = "B：" + enemyColorBSlider.value;
        enemyColorA.text = "A：" + enemyColorASlider.value;
    }

    public void ChangeColorR()
    {
        enemyColorR.text = "R：" + enemyColorRSlider.value;
        enemyColor.r = (byte)enemyColorRSlider.value;
        enemy.GetComponent<Renderer>().material.color = enemyColor;
    }

    public void ChangeColorG()
    {
        enemyColorG.text = "G：" + enemyColorGSlider.value;
        enemyColor.g = (byte)enemyColorGSlider.value;
        enemy.GetComponent<Renderer>().material.color = enemyColor;
    }

    public void ChangeColorB()
    {
        enemyColorB.text = "B：" + enemyColorBSlider.value;
        enemyColor.b = (byte)enemyColorBSlider.value;
        enemy.GetComponent<Renderer>().material.color = enemyColor;
    }

    public void ChangeColorA()
    {
        enemyColorA.text = "A：" + enemyColorASlider.value;
        enemyColor.a = (byte)enemyColorASlider.value;
        enemy.GetComponent<Renderer>().material.color = enemyColor;
    }
}
