using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCloseOpenParameter : MonoBehaviour
{
    public GameObject parameter;
    private Text text;
    private bool open = false;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = "Edit";
        parameter.SetActive(false);
    }

    public void ManageUI()
    {
        if (open)
        {
            text.text = "Edit";
            parameter.SetActive(false);
            open = false;
        }
        else
        {
            text.text = "Close";
            parameter.SetActive(true);
            open = true;
        }
    }
}
