using UnityEngine;

public interface ITaskManager
{
    public void InitializeTask();
    public void SwitchTaskState(TaskState nextState);
    public Vector3 DecideArrivalPosition(TaskBallController ball);
    public float DecideLateralSpeed(GameObject ballObject, TaskBallController ball, float ballSpeedY, Vector3 targetPosition);
    public float DecideDepthSpeed(GameObject ballObject, TaskBallController ball, float ballSpeedY, Vector3 targetPosition);
}
