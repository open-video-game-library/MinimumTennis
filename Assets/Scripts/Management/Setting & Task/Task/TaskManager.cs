using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    private GameObject movingTaskManager;
    [SerializeField]
    private GameObject hittingTaskManager;
    [SerializeField]
    private GameObject rallyingTaskManager;

    private TaskPauseManager pause;

    // Start is called before the first frame update
    void Start()
    {
        pause = GetComponent<TaskPauseManager>();

        movingTaskManager.SetActive(false);
        hittingTaskManager.SetActive(false);
        rallyingTaskManager.SetActive(false);

        switch (Parameters.taskType)
        {
            case TaskType.moving:
                movingTaskManager.SetActive(true);
                break;
            case TaskType.hitting:
                hittingTaskManager.SetActive(true);
                break;
            case TaskType.rallying:
                rallyingTaskManager.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TaskData.controllable = TaskData.taskState == TaskState.Playing;
    }

    // アニメーション内から呼ばれる関数
    public void SwitchPlayingState()
    {
        TaskData.taskManagerInterface.SwitchTaskState(TaskState.Playing);
    }

    public void BackHome()
    {
        TaskData.taskManagerInterface.InitializeTask();
        SceneManager.LoadScene("HomeScene");
    }

    public void RestartGame()
    {
        // Reload this scene
        pause.ExitPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // アニメーション内から呼ばれる関数
    public void LoadResultScene()
    {
        SceneManager.LoadScene("TaskResultScene");
    }
}
