using UnityEngine;
using UnityEngine.UI;

public class HomeSEManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource click;

    [SerializeField]
    private Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        click.Play();
    }
}
