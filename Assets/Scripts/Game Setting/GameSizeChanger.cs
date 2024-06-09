using UnityEngine;
using UnityEngine.UI;

public class GameSizeChanger : MonoBehaviour
{
    [SerializeField]
    private Toggle[] toggles = new Toggle[5];
    private bool[] toggleBools;

    // Start is called before the first frame update
    void Start()
    {
        toggleBools = new bool[toggles.Length];

        // ParametersÇ©ÇÁÉfÅ[É^ÇÃì«Ç›çûÇ›
        toggles[(int)Parameters.gameSize - 1].isOn = true;
    }

    public void ChangeGameSize()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (!toggleBools[i] && toggles[i].isOn)
            {
                // OFF Å® ON Ç…Ç»ÇÈÇ∆Ç´
                Parameters.gameSize = TranslateIntToGameSize(i + 1);
                for (int j = 0; j < toggles.Length; j++) { toggleBools[j] = toggles[j].isOn; }
            }
        }
    }

    private GameSize TranslateIntToGameSize(int intValue)
    {
        return intValue switch
        {
            1 => GameSize.oneGame,
            2 => GameSize.threeGames,
            3 => GameSize.fiveGames,
            4 => GameSize.sevenGames,
            5 => GameSize.nineGames,
            _ => Parameters.gameSize,
        };
    }
}
