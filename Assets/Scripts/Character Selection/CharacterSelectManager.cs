using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum SelectState
{
    firstPlayer = 1,
    secondPlayer = 2
}

public class CharacterSelectManager : MonoBehaviour
{
    private SelectState selectState = SelectState.firstPlayer;

    [SerializeField]
    private GameObject allCharactersObject;
    private AllCharacters allCharacters;

    private GameObject[] players;

    private GameObject[] firstCharactersDisplay;
    private GameObject[] secondCharactersDisplay;

    [SerializeField]
    private TMP_Text characterNameText;
    [SerializeField]
    private TMP_Text playerNumText;

    [SerializeField]
    private CharacterSelectionBGMManager bgmManager;

    private CharacterSelector characterSelector;

    private readonly float characterSpan = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = SystemParameters.fps;

        allCharacters = allCharactersObject.GetComponent<AllCharacters>();

        characterSelector = GetComponent<CharacterSelector>();

        players = allCharacters.characterObjects.characters;

        firstCharactersDisplay = new GameObject[players.Length];
        secondCharactersDisplay = new GameObject[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            firstCharactersDisplay[i] = Instantiate(players[i]);
            firstCharactersDisplay[i].name = players[i].name;
            firstCharactersDisplay[i].AddComponent<SelectionCharacterController>();

            secondCharactersDisplay[i] = Instantiate(players[i]);
            secondCharactersDisplay[i].name = players[i].name;
            secondCharactersDisplay[i].AddComponent<SelectionCharacterController>();
        }

        for (int i = 0; i < firstCharactersDisplay.Length; i++)
        {
            firstCharactersDisplay[i].transform.position = new Vector3(i * characterSpan, 0.0f, 30.0f);
            firstCharactersDisplay[i].transform.eulerAngles = new Vector3(0.0f, 170.0f + 24.0f, 0.0f);
        }
        for (int i = 0; i < secondCharactersDisplay.Length; i++)
        {
            secondCharactersDisplay[i].transform.position = new Vector3(i * characterSpan, 0.0f, 90.0f);
            secondCharactersDisplay[i].transform.eulerAngles = new Vector3(0.0f, 170.0f + 24.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Select first character
        if (selectState == SelectState.firstPlayer)
        {
            characterSelector.SelectFirstCharacter(firstCharactersDisplay.Length, characterSpan);
            characterNameText.text = firstCharactersDisplay[characterSelector.currentNum].name;
        }
        // Select second character
        else if (selectState == SelectState.secondPlayer)
        {
            characterSelector.SelectSecondCharacter(secondCharactersDisplay.Length, characterSpan);
            characterNameText.text = secondCharactersDisplay[characterSelector.currentNum].name;
        }

        playerNumText.text = "Player " + (int)selectState;
    }

    public void NextCharacter()
    {
        if (selectState == SelectState.firstPlayer)
        {
            CharacterGenerator.selectedCharacterData[0] = firstCharactersDisplay[characterSelector.currentNum].GetComponent<CharacterData>().character;
            characterSelector.currentNum = 0;
            selectState = SelectState.secondPlayer;
        }
        else if (selectState == SelectState.secondPlayer)
        {
            CharacterGenerator.selectedCharacterData[1] = secondCharactersDisplay[characterSelector.currentNum].GetComponent<CharacterData>().character;
            bgmManager.PauseBGM();
            StartCoroutine(LoadAfterWaiting("GameSettingScene"));
        }
    }

    public void BackCharacter()
    {
        if (selectState == SelectState.firstPlayer)
        {
            CharacterGenerator.selectedCharacterData[0] = null;
            bgmManager.PauseBGM();
            StartCoroutine(LoadAfterWaiting("HomeScene"));
        }
        else if (selectState == SelectState.secondPlayer)
        {
            CharacterGenerator.selectedCharacterData[1] = null;
            characterSelector.currentNum = 0;
            selectState = SelectState.firstPlayer;
        }
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
