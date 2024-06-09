using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionSEManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource selectClick;
    [SerializeField]
    private AudioSource decideClick;

    [SerializeField]
    private Button[] selectButtons;
    [SerializeField]
    private Button[] decideButtons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < selectButtons.Length; i++) { selectButtons[i].onClick.AddListener(PlaySelectClickSound); }
        for (int i = 0; i < decideButtons.Length; i++) { decideButtons[i].onClick.AddListener(PlayDecideClickSound); }
    }

    private void PlaySelectClickSound()
    {
        selectClick.Play();
    }

    private void PlayDecideClickSound()
    {
        decideClick.Play();
    }
}
