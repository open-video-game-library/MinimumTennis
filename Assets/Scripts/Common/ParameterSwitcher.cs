using UnityEngine;
using UnityEngine.UI;

public class ParameterSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject parameter;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = "Edit";
        parameter.SetActive(false);
    }

    public void SwitchUI()
    {
        if (parameter.activeSelf)
        {
            text.text = "Edit";
            parameter.SetActive(false);
        }
        else
        {
            text.text = "Close";
            parameter.SetActive(true);
        }
    }
}
