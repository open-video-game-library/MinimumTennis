using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionBGMManager : MonoBehaviour
{
    private AudioSource bgm;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponent<AudioSource>();
    }

    public void PauseBGM()
    {
        bgm.Pause();
    }
}
