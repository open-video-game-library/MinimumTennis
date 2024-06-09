using UnityEngine;
using UnityEngine.UI;

public class GameSettingSEManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource selectClick;
    [SerializeField]
    private AudioSource decideClick;

    private Toggle[] toggles;
    [SerializeField]
    private Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] toggleObjects = GameObject.FindGameObjectsWithTag("Toggle");
        toggles = new Toggle[toggleObjects.Length];
        for (int i = 0; i < toggles.Length; i++) { toggles[i] = toggleObjects[i].GetComponent<Toggle>(); }

        for (int i = 0; i < toggles.Length; i++) { toggles[i].onValueChanged.AddListener(PlaySelectClickSound); }
        for (int i = 0; i < buttons.Length; i++) { buttons[i].onClick.AddListener(PlayDecideClickSound); }
    }

    private void PlaySelectClickSound(bool selected)
    {
        if (selected) { selectClick.Play(); }
    }

    private void PlayDecideClickSound()
    {
        decideClick.Play();
    }
}
