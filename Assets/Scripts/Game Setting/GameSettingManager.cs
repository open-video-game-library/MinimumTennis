using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettingManager : MonoBehaviour
{
    public static InputMethod inputMethod;

    void Start()
    {
        Application.targetFrameRate = SystemParameters.fps;
    }

    public void LoadCharacterSelectScene()
    {
        // キャラクタ選択シーンに戻る
        StartCoroutine(LoadAfterWaiting("CharacterSelectScene"));
    }

    public void StartGame()
    {
        switch (Parameters.playMode)
        {
            case PlayMode.normal:
                StartCoroutine(LoadAfterWaiting("TennisScene"));
                break;
            case PlayMode.competition:
                StartCoroutine(LoadAfterWaiting("TennisScene"));
                break;
            case PlayMode.setting:
                StartCoroutine(LoadAfterWaiting("ParameterSettingScene"));
                break;
            case PlayMode.task:
                StartCoroutine(LoadAfterWaiting("TaskScene"));
                break;
        }
    }

    private IEnumerator LoadAfterWaiting(string sceneName)
    {
        // 入力を無効化
        DisableInput();

        // 1秒待機
        yield return new WaitForSeconds(1);

        // 入力を有効化
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
        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            if (toggle.gameObject.TryGetComponent(out IConnectionManager connectionManager))
            {
                // Interfaceを経由して、コントローラの接続状態を確認する処理を停止させる
                connectionManager.Active = false;
            }

            toggle.interactable = false;
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
        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            if (toggle.gameObject.TryGetComponent(out IConnectionManager connectionManager))
            {
                // Interfaceを経由して、コントローラの接続状態を確認する処理を再開させる
                connectionManager.Active = true;
            }

            toggle.interactable = true;
        }
    }
}
