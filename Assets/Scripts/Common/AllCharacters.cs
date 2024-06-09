using UnityEngine;

[System.Serializable]
public class CharacterObjects
{
    public GameObject[] characters;

    public CharacterObjects(GameObject[] _characters)
    {
        characters = _characters;
    }
}

public class AllCharacters : MonoBehaviour
{
    // 以下のCharacterObjectsクラスの変数に、Unityエディタから値を設定する。
    public CharacterObjects characterObjects;

    void Awake()
    {
        // CharacterNumberが被らないように、ここで自動的に番号を振ってくれる処理
        characterObjects.characters = SetData(characterObjects.characters);
    }

    private GameObject[] SetData(GameObject[] characterObjects)
    {
        GameObject[] returnObjects = characterObjects;

        for (int i = 0; i < returnObjects.Length; i++)
        {
            CharacterData characterData = returnObjects[i].GetComponent<CharacterData>();
            Character character = characterData.character;

            character.characterNumber = i;
        }

        return returnObjects;
    }
}
