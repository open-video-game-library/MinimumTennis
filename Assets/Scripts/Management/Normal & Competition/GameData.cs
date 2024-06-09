using UnityEngine;
public enum GameState
{
    Start,
    Prepare,
    Playing,
    Replay,
    End
}

public class Area
{
    public float xNegativeLimit;
    public float xPositiveLimit;
    public float zNegativeLimit;
    public float zPositiveLimit;

    public Area(float _xNegativeLimit, float _xPositiveLimit, float _zNegativeLimit, float _zPositiveLimit)
    {
        xNegativeLimit = _xNegativeLimit;
        xPositiveLimit = _xPositiveLimit;
        zNegativeLimit = _zNegativeLimit;
        zPositiveLimit = _zPositiveLimit;
    }

    public bool CheckInside(float x, float z)
    {
        bool checkResult;

        checkResult = 
            xNegativeLimit <= x && x <= xPositiveLimit 
            && zNegativeLimit <= z && z <= zPositiveLimit;

        return checkResult;
    }

    public bool CheckInside(float x, float z, float margin)
    {
        bool checkResult;

        checkResult =
            xNegativeLimit - margin <= x && x <= xPositiveLimit + margin
            && zNegativeLimit - margin <= z && z <= zPositiveLimit + margin;

        return checkResult;
    }
}

public enum ServePosition
{
    deuce = 1,
    advantage = -1
}

public enum FoulState
{
    Fault,
    DoubleFault,
    Net,
    Out,
    TwoBounds,
    NoFoul
}

public static class GameData
{
    // Game status data
    public static GameState gameState;
    public static bool controllable;
    public static bool replayCancel;
    public static bool pause;

    // Environment data
    public static readonly Area courtArea = new Area(-16.8f, 16.8f, -47.54f, 47.54f);
    public static readonly Area[] playersCourtArea = { new Area(-16.8f, 16.8f, -47.54f, 0.0f), new Area(-16.8f, 16.8f, 0.0f, 47.54f) };
    public static readonly float netHight = 4.0f;

    // Movable area data
    public static readonly Area[] movableArea = { new Area(-32.0f, 32.0f, -80.0f, -10.0f), new Area(-32.0f, 32.0f, 10.0f, 80.0f) };

    // Character data
    public static GameObject character1;
    public static GameObject character2;

    // Score data
    public static int character1Score;
    public static int character2Score;
    public static int character1GameCount;
    public static int character2GameCount;
    public static string[] character1ScoreResult;
    public static string[] character2ScoreResult;

    // Judge data
    public static FoulState foul;
    public static string server;
    public static string lastShooter;
    public static string pointLoser;
    public static bool isToss;
    public static bool isBreaked;
    public static bool isServeIn;
    public static bool isOut;
    public static bool isNet;
    public static int rallyCount;
    public static int ballBoundCount;
    public static int faultCount;
    public static ServePosition servePosition;
    public static int ballAmount;

    // CSV data
    public static CSVData csv;
}
