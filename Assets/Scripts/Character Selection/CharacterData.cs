using UnityEngine;

public enum Players
{
    p1 = 0,
    p2 = 1
}

[System.Serializable]
public class Character
{
    public int characterNumber;

    public Character(int _characterNumber)
    {
        characterNumber = _characterNumber;
    }
}

public class CharacterData : MonoBehaviour
{
    public Character character;

    [System.NonSerialized]
    public Players players;
}
