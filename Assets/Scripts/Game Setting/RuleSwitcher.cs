using UnityEngine;

public class RuleSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject normalRuleObject;

    [SerializeField]
    private GameObject taskRuleObject;

    // Start is called before the first frame update
    void Start()
    {
        // Normalモード、Competitionモードの場合に対象オブジェクトをアクティブ化
        normalRuleObject.SetActive(Parameters.playMode == PlayMode.normal || Parameters.playMode == PlayMode.competition);

        // Taskモードの場合に対象オブジェクトをアクティブ化
        taskRuleObject.SetActive(Parameters.playMode == PlayMode.task);
    }
}
