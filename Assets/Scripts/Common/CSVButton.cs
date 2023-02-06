using UnityEngine;
using UnityEngine.UI;

public class CSVButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) { GetComponent<Button>().interactable = true; }
        else { GetComponent<Button>().interactable = false; }
    }
}
