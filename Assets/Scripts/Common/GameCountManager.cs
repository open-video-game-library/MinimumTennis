using UnityEngine;

public class GameCountManager : MonoBehaviour
{
    [SerializeField]
    private GameObject activeIconPrefab;
    [SerializeField]
    private GameObject inactiveIconPrefab;

    [SerializeField]
    private GameObject character1Panel;
    [SerializeField]
    private GameObject character2Panel;

    private GameObject[] character1Count;
    private GameObject[] character2Count;

    // Start is called before the first frame update
    void Start()
    {
        character1Count = new GameObject[GameSize.gameSize];
        character2Count = new GameObject[GameSize.gameSize];
        UpdateCharacter1Count();
        UpdateCharacter2Count();
    }

    void OnEnable()
    {
        if (!GameManager.isBreaked) { return; }
        UpdateCharacter1Count();
        UpdateCharacter2Count();
    }

    private void UpdateCharacter1Count()
    {
        for (int i = 0; i < GameSize.gameSize; i++)
        {
            if (i < GameManager.character1GameCount)
            {
                Destroy(character1Count[i]);
                GameObject icon = Instantiate(activeIconPrefab, character1Panel.transform);
                icon.transform.localPosition = new Vector3(-105.0f + i * 55.0f, 0.0f, 0.0f);
                character1Count[i] = icon;
            }
            else if (i >= GameManager.character1GameCount)
            {
                Destroy(character1Count[i]);
                GameObject icon = Instantiate(inactiveIconPrefab, character1Panel.transform);
                icon.transform.localPosition = new Vector3(-105.0f + i * 55.0f, 0.0f, 0.0f);
                character1Count[i] = icon;
            }
        }
    }

    private void UpdateCharacter2Count()
    {
        for (int i = 0; i < GameSize.gameSize; i++)
        {
            if (i < GameManager.character2GameCount)
            {
                Destroy(character2Count[i]);
                GameObject icon = Instantiate(activeIconPrefab, character2Panel.transform);
                icon.transform.localPosition = new Vector3(105.0f - i * 55.0f, 0.0f, 0.0f);
                character2Count[i] = icon;
            }
            else if (i >= GameManager.character2GameCount)
            {
                Destroy(character2Count[i]);
                GameObject icon = Instantiate(inactiveIconPrefab, character2Panel.transform);
                icon.transform.localPosition = new Vector3(105.0f - i * 55.0f, 0.0f, 0.0f);
                character2Count[i] = icon;
            }
        }
    }
}
