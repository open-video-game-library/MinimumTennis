using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoyconConnectionManager : MonoBehaviour, IConnectionManager
{
    public bool Active { get; set; }

    [SerializeField]
    private Image image;
    private Toggle toggle;
    private TMP_Text statusText;

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;

    // Start is called before the first frame update
    void Start()
    {
        Active = true;

        toggle = GetComponent<Toggle>();
        statusText = GetComponent<TMP_Text>();

        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) { return; }

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    // Update is called once per frame
    void Update()
    {
        if (ManageConnection())
        {
            image.color = new Color(1.0f, 1.0f, 1.0f);
            toggle.interactable = Active;
            statusText.text = "Joy-Con" + "\n" + "(connected)";
        }
        else
        {
            image.color = new Color(0.20f, 0.20f, 0.20f);
            toggle.interactable = false;
            statusText.text = "Joy-Con" + "\n" + "(not connected)";
        }
    }

    public bool ManageConnection()
    {
        // ç∂âEÇÃJoy-ConÇ™óºï˚åqÇ™Ç¡ÇƒÇ¢ÇÈÇ©îªíË
        return (m_joyconL != null) && (m_joyconR != null);
    }

    public InputMethod ReturnInputMethod()
    {
        return InputMethod.motion;
    }
}
