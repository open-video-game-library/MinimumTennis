using UnityEngine;
using UnityEngine.UI;

public class HomeBGMManager : MonoBehaviour
{
    private AudioSource bgm;

    [SerializeField]
    private Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponent<AudioSource>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(PauseBGM);
        }
    }

    private void PauseBGM()
    {
        bgm.Pause();
    }
}
