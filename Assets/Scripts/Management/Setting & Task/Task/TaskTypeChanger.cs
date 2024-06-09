using UnityEngine;
using UnityEngine.UI;

public class TaskTypeChanger : MonoBehaviour
{
    [SerializeField]
    private Toggle movingToggle;

    [SerializeField]
    private Toggle hittingToggle;

    [SerializeField]
    private Toggle rallyingToggle;

    // Start is called before the first frame update
    void Start()
    {
        // ParametersÇ©ÇÁÉfÅ[É^ÇÃì«Ç›çûÇ›
        if (Parameters.taskType == TaskType.moving) { movingToggle.isOn = true; }
        else if (Parameters.taskType == TaskType.hitting) { hittingToggle.isOn = true; }
        else if (Parameters.taskType == TaskType.rallying) { rallyingToggle.isOn = true; }
    }

    public void ChangeTaskType()
    {
        if (movingToggle.isOn) { Parameters.taskType = TaskType.moving; }
        else if (hittingToggle.isOn) { Parameters.taskType = TaskType.hitting; }
        else if (rallyingToggle.isOn) { Parameters.taskType = TaskType.rallying; }
    }
}
