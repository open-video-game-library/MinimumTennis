using UnityEngine;
using TMPro;

public class GameInfoPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private int gameSize;

    // Start is called before the first frame update
    void Start()
    {
        gameSize = (int)Parameters.gameSize;

        if (gameSize == 1) { text.text = "Score " + gameSize + " game to win!"; }
        else { text.text = "Score " + gameSize + " games to win!"; }
    }
}
