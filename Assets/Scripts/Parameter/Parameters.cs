using UnityEngine;

public enum PlayMode
{
    normal,
    competition,
    setting,
    task
}

public enum GameSize
{
    oneGame = 1,
    threeGames = 2,
    fiveGames = 3,
    sevenGames = 4,
    nineGames = 5
}

public enum TaskType
{
    moving,
    hitting,
    rallying,
    setting
}

public enum DominantHand
{
    right = -1,
    left = 1
}

public enum InputMethod
{
    keyboard,
    gamepad,
    motion,
    none
}

public class Parameters : MonoBehaviour
{
    // Players parameters ---------------------------------
    public static float[] maximumSpeed = { 30.0f, 30.0f };
    public static float[] acceleration = { 1.0f, 1.0f };
    public static float[] ballSpeed = { 1.0f, 1.0f };
    public static float[] reactionDelay = { 0.30f, 0.30f };
    public static float[] distance = { 0.50f, 0.50f };
    public static float motionThrehold = 1.0f;

    // Game parameters ------------------------------------

    // Home画面でのボタン操作から値を受け取る
    public static PlayMode playMode = PlayMode.normal;

    // 各モードのSetting画面でのトグル選択から値を受け取る
    public static DominantHand[] charactersDominantHand = { DominantHand.right, DominantHand.right };

    // Normal・CompetitionモードのSetting画面でのトグル選択から値を受け取る
    public static GameSize gameSize = GameSize.oneGame;

    // TaskモードのSetting画面でのトグル選択から値を受け取る
    public static TaskType taskType = TaskType.moving;

    // 各モードのSetting画面でのトグル選択から値を受け取る
    public static InputMethod[] inputMethod = { InputMethod.keyboard, InputMethod.none };
}
