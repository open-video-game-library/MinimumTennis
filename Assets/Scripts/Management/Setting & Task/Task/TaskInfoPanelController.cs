using UnityEngine;
using TMPro;

public class TaskInfoPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        switch (Parameters.taskType)
        {
            case TaskType.moving:
                text.text = "Catch up with the ball!";
                break;
            case TaskType.hitting:
                text.text = "Aim and shoot the ball!";
                break;
            case TaskType.rallying:
                text.text = "Keep the rally going!";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TaskData.taskState == TaskState.End)
        {
            text.text = "Finish!!";
        }
    }
}
