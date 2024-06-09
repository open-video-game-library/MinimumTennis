using UnityEngine;

public enum TaskState
{
    Start,
    Prepare,
    Playing,
    End
}

public class TaskData
{
    // Task status data
    public static TaskState taskState;
    public static bool controllable;
    public static bool pause;

    // Environment data
    public static readonly Area courtArea = new Area(-16.8f, 16.8f, -47.54f, 47.54f);
    public static readonly Area[] playersCourtArea = { new Area(-16.8f, 16.8f, -47.54f, 0.0f), new Area(-16.8f, 16.8f, 0.0f, 47.54f) };
    public static readonly float netHight = 4.0f;

    // Movable area data
    public static readonly Area[] movableArea = { new Area(-32.0f, 32.0f, -80.0f, -10.0f), new Area(-32.0f, 32.0f, 10.0f, 80.0f) };

    // Task manager data
    public static ITaskManager taskManagerInterface;

    // Character data
    public static GameObject character1;
    public static GameObject character2;

    // Character default position
    public static readonly Vector3 character1DefalutPosition = new Vector3(0.0f, 0.0f, -49.0f);
    public static readonly Vector3 character2DefalutPosition = new Vector3(0.0f, 0.0f, 49.0f);

    // Judge data
    public static FoulState foul;
    public static string lastShooter;
    public static bool isOut;
    public static bool isNet;
    public static int rallyCount;
    public static int ballBoundCount;
    public static int ballAmount;
}
