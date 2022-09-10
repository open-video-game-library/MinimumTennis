using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControllerAmountCounter : MonoBehaviour
{
    private Text controllerCounter;

    private void Start()
    {
        controllerCounter = gameObject.GetComponent<Text>();
    }

    private void Update()
    {
        controllerCounter.text = "The number of registered controllers is " + Gamepad.all.Count + ".";
    }
}
