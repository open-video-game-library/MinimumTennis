using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GamepadConnectionManager : MonoBehaviour, IConnectionManager
{
    public bool Active { get; set; }

    [SerializeField]
    private Image image;
    private Toggle toggle;
    private TMP_Text statusText;

    [SerializeField]
    private Players players;

    // Start is called before the first frame update
    void Start()
    {
        Active = true;

        toggle = GetComponent<Toggle>();
        statusText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Parameters.playMode == PlayMode.competition)
        {
            if (ManageConnection())
            {
                image.color = new Color(1.0f, 1.0f, 1.0f);
                toggle.interactable = Active;
                statusText.text = "P" + ((int)players + 1) + " Gamepad" + "\n" + "(connected)";
            }
            else
            {
                image.color = new Color(0.20f, 0.20f, 0.20f);
                toggle.interactable = false;
                statusText.text = "P" + ((int)players + 1) + " Gamepad" + "\n" + "(not connected)";
            }
        }
        else
        {
            if (ManageConnection())
            {
                image.color = new Color(1.0f, 1.0f, 1.0f);
                toggle.interactable = true;
                statusText.text = "Gamepad" + "\n" + "(connected)";
            }
            else
            {
                image.color = new Color(0.20f, 0.20f, 0.20f);
                toggle.interactable = false;
                statusText.text = "Gamepad" + "\n" + "(not connected)";
            }
        }
    }

    public bool ManageConnection()
    {
        int connectedControllerNum = Gamepad.all.Count;

        if (connectedControllerNum == 0) { return false; }
        else if (connectedControllerNum >= 1 && players == Players.p1) { return Gamepad.all[(int)players] != null; }
        else if (connectedControllerNum >= 2 && players == Players.p2) { return Gamepad.all[(int)players] != null; }
        else { return false; }
    }

    public InputMethod ReturnInputMethod()
    {
        return InputMethod.gamepad;
    }
}
