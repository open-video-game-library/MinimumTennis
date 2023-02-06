using UnityEngine;
using UnityEngine.UI;

public class GameSize : MonoBehaviour
{
    [System.NonSerialized]
    public static int gameSize = 3;

    private Slider gameSizeSlider;

    public GameObject gameSizeText;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        gameSizeSlider = GetComponent<Slider>();
        text = gameSizeText.GetComponent<Text>();

        gameSizeSlider.value = gameSize;
        if (gameSize == 1) { text.text = "First to a ''" + gameSize + "'' game, win the game."; }
        else { text.text = "First to ''" + gameSize + "'' games, win the game."; }
    }

    public void ChangeGameSize()
    {
        gameSize = (int)gameSizeSlider.value;

        if (gameSize == 1) { text.text = "First to a ''" + gameSize + "'' game, win the game."; }
        else { text.text = "First to ''" + gameSize + "'' games, win the game."; }
    }
}
