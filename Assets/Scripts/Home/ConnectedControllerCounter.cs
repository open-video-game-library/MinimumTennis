using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ConnectedControllerCounter : MonoBehaviour
{
    private Text controllerCount;

    private void Start()
    {
        controllerCount = gameObject.GetComponent<Text>();
    }

    private void Update()
    {
        controllerCount.text = "The number of registered controllers is " + Gamepad.all.Count + ".";
    }
}
