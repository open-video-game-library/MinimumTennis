using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public static Character[] selectedCharacterData = new Character[2];

    public static Vector3[] playerDefaultPosition = { new Vector3(8.0f, 0.0f, -49.0f), new Vector3(-8.0f, 0.0f, 49.0f) };
    public static Vector3[] playerDefaultRotation = { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 180.0f, 0.0f) };

    [SerializeField]
    private GameObject[] defaultCharacters = new GameObject[2];

    [SerializeField]
    private GameObject allCharactersObjectRight;
    private AllCharacters allCharactersRight;

    [SerializeField]
    private GameObject allCharactersObjectLeft;
    private AllCharacters allCharactersLeft;

    // 右利きのアバタ群
    private GameObject[] playersRight;

    // 左利きのアバタ群
    private GameObject[] playersLeft;

    private GameObject[] characters = new GameObject[2];
    private static GameObject[] returnCharacters = new GameObject[2];

    // Start is called before the first frame update
    void Awake()
    {
        allCharactersRight = allCharactersObjectRight.GetComponent<AllCharacters>();
        allCharactersLeft = allCharactersObjectLeft.GetComponent<AllCharacters>();

        // 右利きのアバタ群の読み込み
        playersRight = allCharactersRight.characterObjects.characters;

        // 左利きのアバタ群の読み込み
        playersLeft = allCharactersLeft.characterObjects.characters;

        for (int i = 0; i < characters.Length; i++)
        {
            if (selectedCharacterData[i] != null) { characters[i] = SearchCharacter(selectedCharacterData[i], i, Parameters.charactersDominantHand[i]); }
            else
            {
                Debug.Log("Character data is not set!");
                characters[i] = defaultCharacters[i];
            }
            returnCharacters[i] = Instantiate(characters[i]);

            // returnCharacters[i]に、キャラクター操作に必要なコンポーネントをアタッチする
            AddPlayerComponent(i);

            // キャラクターの初期位置と初期回転を設定する
            returnCharacters[i].transform.position = playerDefaultPosition[i];
            returnCharacters[i].transform.eulerAngles = playerDefaultRotation[i];

            returnCharacters[i].name = characters[i].name;
        }

        // 名前の重複がないように、同じ名前の場合はナンバリングする
        if (returnCharacters[0].name == returnCharacters[1].name)
        {
            returnCharacters[0].name = returnCharacters[0].name + " 1";
            returnCharacters[1].name = returnCharacters[1].name + " 2";
        }

        returnCharacters[0].GetComponent<CharacterData>().players = Players.p1;
        returnCharacters[1].GetComponent<CharacterData>().players = Players.p2;
    }

    public static GameObject GetCharacter(int characterNum)
    {
        return returnCharacters[characterNum];
    }

    private GameObject SearchCharacter(Character characterData, int characterNum, DominantHand dominantHand)
    {
        GameObject targetCharacter = defaultCharacters[characterNum];

        int targetNumber = characterData.characterNumber;

        GameObject[] players;

        if (dominantHand == DominantHand.left) { players = playersLeft; }
        else { players = playersRight; }

        for (int i = 0; i < players.Length; i++)
        {
            Character comparisonData = players[i].GetComponent<CharacterData>().character;
            if (targetNumber == comparisonData.characterNumber) { targetCharacter = players[i]; }
        }

        return targetCharacter;
    }

    private void AddPlayerComponent(int playerNum)
    {
        // プレイヤの移動を制御するコンポーネント
        returnCharacters[playerNum].GetComponent<Move>().player = (Players)playerNum;
        // プレイヤのスイングを制御するコンポーネント
        returnCharacters[playerNum].GetComponent<Shot>().player = (Players)playerNum;

        if (Parameters.playMode == PlayMode.normal || Parameters.playMode == PlayMode.competition)
        {
            // 試合形式のモードで、プレイヤの動きを制御するコンポーネント
            returnCharacters[playerNum].AddComponent<PlayerNormalController>().player = (Players)playerNum;

            // プレイヤを自動で操作するコンポーネント
            returnCharacters[playerNum].AddComponent<PlayerNormalAI>().player = (Players)playerNum;
        }
        else if (Parameters.playMode == PlayMode.setting || Parameters.playMode == PlayMode.task)
        {
            // タスク形式のモードで、プレイヤの動きを制御するコンポーネント
            returnCharacters[playerNum].AddComponent<PlayerTaskController>().player = (Players)playerNum;

            // プレイヤを自動で操作するコンポーネント
            returnCharacters[playerNum].AddComponent<PlayerTaskAI>().player = (Players)playerNum;
        }

        switch (Parameters.playMode)
        {
            case PlayMode.normal:

                PlayerNormalAI normalAI = returnCharacters[playerNum].GetComponent<PlayerNormalAI>();

                switch (Parameters.inputMethod[playerNum])
                {
                    case InputMethod.keyboard:
                        returnCharacters[playerNum].AddComponent<KeyboardInputManager>();
                        normalAI.autoMove = false;
                        normalAI.autoShot = false;
                        break;
                    case InputMethod.gamepad:
                        returnCharacters[playerNum].AddComponent<GamepadInputManager>();
                        normalAI.autoMove = false;
                        normalAI.autoShot = false;
                        break;
                    case InputMethod.motion:
                        returnCharacters[playerNum].AddComponent<JoyconInputManager>();
                        returnCharacters[playerNum].AddComponent<JoyconManager>();
                        normalAI.autoMove = true;
                        normalAI.autoShot = false;
                        break;
                    case InputMethod.none:
                        normalAI.autoMove = true;
                        normalAI.autoShot = true;
                        break;
                }
                break;
            case PlayMode.competition:

                PlayerNormalAI competitionAI = returnCharacters[playerNum].GetComponent<PlayerNormalAI>();

                switch (Parameters.inputMethod[playerNum])
                {
                    case InputMethod.keyboard:
                        // Setting画面で選択不可
                        break;
                    case InputMethod.gamepad:
                        returnCharacters[playerNum].AddComponent<GamepadInputManager>();
                        competitionAI.autoMove = false;
                        competitionAI.autoShot = false;
                        break;
                    case InputMethod.motion:
                        // Setting画面で選択不可
                        break;
                    case InputMethod.none:
                        // Setting画面で選択不可
                        break;
                }
                break;
            case PlayMode.setting:

                PlayerTaskAI settingAI = returnCharacters[playerNum].GetComponent<PlayerTaskAI>();

                switch (Parameters.inputMethod[playerNum])
                {
                    case InputMethod.keyboard:
                        returnCharacters[playerNum].AddComponent<KeyboardInputManager>();
                        settingAI.autoMove = false;
                        settingAI.autoShot = false;
                        break;
                    case InputMethod.gamepad:
                        returnCharacters[playerNum].AddComponent<GamepadInputManager>();
                        settingAI.autoMove = false;
                        settingAI.autoShot = false;
                        break;
                    case InputMethod.motion:
                        returnCharacters[playerNum].AddComponent<JoyconInputManager>();
                        returnCharacters[playerNum].AddComponent<JoyconManager>();
                        settingAI.autoMove = true;
                        settingAI.autoShot = false;
                        break;
                    case InputMethod.none:
                        // 1PはSetting画面で選択不可
                        settingAI.autoMove = true;
                        settingAI.autoShot = true;
                        break;
                }
                break;
            case PlayMode.task:

                PlayerTaskAI taskAI = returnCharacters[playerNum].GetComponent<PlayerTaskAI>();

                switch (Parameters.inputMethod[playerNum])
                {
                    case InputMethod.keyboard:
                        returnCharacters[playerNum].AddComponent<KeyboardInputManager>();
                        taskAI.autoMove = false;
                        taskAI.autoShot = false;
                        break;
                    case InputMethod.gamepad:
                        returnCharacters[playerNum].AddComponent<GamepadInputManager>();
                        taskAI.autoMove = false;
                        taskAI.autoShot = false;
                        break;
                    case InputMethod.motion:
                        returnCharacters[playerNum].AddComponent<JoyconInputManager>();
                        returnCharacters[playerNum].AddComponent<JoyconManager>();
                        taskAI.autoMove = true;
                        taskAI.autoShot = false;
                        break;
                    case InputMethod.none:
                        // 1PはSetting画面で選択不可
                        taskAI.autoMove = true;
                        taskAI.autoShot = true;
                        break;
                }
                break;
        }
    }
}
