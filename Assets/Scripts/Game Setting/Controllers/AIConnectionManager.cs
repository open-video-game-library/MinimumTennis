using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIConnectionManager : MonoBehaviour, IConnectionManager
{
    public bool Active { get; set; }

    [SerializeField]
    private Image image;
    private Toggle toggle;
    private TMP_Text statusText;

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
        if (ManageConnection())
        {
            image.color = new Color(1.0f, 1.0f, 1.0f);
            toggle.interactable = Active;
            statusText.text = "Auto" + "\n" + "(available)";
        }
        else
        {
            image.color = new Color(0.20f, 0.20f, 0.20f);
            toggle.interactable = false;
            statusText.text = "Auto" + "\n" + "(not available)";
        }
    }

    public bool ManageConnection()
    {
        return true;
    }

    public InputMethod ReturnInputMethod()
    {
        return InputMethod.none;
    }
}
