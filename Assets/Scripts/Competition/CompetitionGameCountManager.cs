using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitionGameCountManager : MonoBehaviour
{
    public GameObject[] playerCount;
    public GameObject[] enemyCount;

    public GameObject activeIcon_prefab;
    public GameObject not_activeIcon_prefab;

    public GameObject gameManager;
    private CompetitionGameManager manager;

    public GameObject playerPanel;
    public GameObject enemyPanel;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameManager.GetComponent<CompetitionGameManager>();

        playerCount = new GameObject[GameSize.gameSize];
        enemyCount = new GameObject[GameSize.gameSize];

        UpdatePlayerCount();
        UpdateEnemyCount();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.isBREAKED)
        {
            if (manager.whoBREAK == "Player1")
            {
                manager.playerGameCount++;
                UpdatePlayerCount();

                manager.whoBREAK = null;
            }
            else if (manager.whoBREAK == "Player2")
            {
                manager.enemyGameCount++;
                UpdateEnemyCount();

                manager.whoBREAK = null;
            }
        }
    }

    public void UpdatePlayerCount()
    {
        for (int i = 0; i < GameSize.gameSize; i++)
        {
            if (i < manager.playerGameCount)
            {
                Destroy(playerCount[i]);
                GameObject icon = Instantiate(activeIcon_prefab, playerPanel.transform);
                icon.transform.localPosition = new Vector3(-105.0f + i * 55.0f, 0.0f, 0.0f);
                playerCount[i] = icon;
            }
            else if (i >= manager.playerGameCount)
            {
                Destroy(playerCount[i]);
                GameObject icon = Instantiate(not_activeIcon_prefab, playerPanel.transform);
                icon.transform.localPosition = new Vector3(-105.0f + i * 55.0f, 0.0f, 0.0f);
                playerCount[i] = icon;
            }
        }
    }

    public void UpdateEnemyCount()
    {
        for (int i = 0; i < GameSize.gameSize; i++)
        {
            if (i < manager.enemyGameCount)
            {
                Destroy(enemyCount[i]);
                GameObject icon = Instantiate(activeIcon_prefab, enemyPanel.transform);
                icon.transform.localPosition = new Vector3(105.0f - i * 55.0f, 0.0f, 0.0f);
                enemyCount[i] = icon;
            }
            else if (i >= manager.enemyGameCount)
            {
                Destroy(enemyCount[i]);
                GameObject icon = Instantiate(not_activeIcon_prefab, enemyPanel.transform);
                icon.transform.localPosition = new Vector3(105.0f - i * 55.0f, 0.0f, 0.0f);
                enemyCount[i] = icon;
            }
        }
    }
}
