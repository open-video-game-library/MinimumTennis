using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = SystemParameters.fps;
    }

    public void StartNormalMode()
    {
        Parameters.playMode = PlayMode.normal;
        StartCoroutine(LoadAfterWaiting("CharacterSelectScene"));
    }

    public void StartCompetitionMode()
    {
        Parameters.playMode = PlayMode.competition;
        StartCoroutine(LoadAfterWaiting("CharacterSelectScene"));
    }

    public void StartSettingMode()
    {
        Parameters.playMode = PlayMode.setting;
        StartCoroutine(LoadAfterWaiting("CharacterSelectScene"));
    }

    public void StartTaskMode()
    {
        Parameters.playMode = PlayMode.task;
        StartCoroutine(LoadAfterWaiting("CharacterSelectScene"));
    }

    private IEnumerator LoadAfterWaiting(string sceneName)
    {
        // “ü—Í‚ð–³Œø‰»
        DisableInput();

        // 1•b‘Ò‹@
        yield return new WaitForSeconds(1);

        // “ü—Í‚ð—LŒø‰»
        EnableInput();
        SceneManager.LoadScene(sceneName);
    }

    private void DisableInput()
    {
        foreach (var input in FindObjectsOfType<InputField>())
        {
            input.interactable = false;
        }
        foreach (var button in FindObjectsOfType<Button>())
        {
            button.interactable = false;
        }
    }

    private void EnableInput()
    {
        foreach (var input in FindObjectsOfType<InputField>())
        {
            input.interactable = true;
        }
        foreach (var button in FindObjectsOfType<Button>())
        {
            button.interactable = true;
        }
    }
}
