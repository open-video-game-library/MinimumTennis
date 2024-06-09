using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Manage()
    {
        // "Finish!!"と表示する
        animator.SetTrigger("Finish");
    }

    // アニメーションの中から呼ばれる関数
    public void LoadResultScene()
    {
        ResultManager.character1Name = GameData.character1.name;
        ResultManager.character2Name = GameData.character2.name;

        ResultManager.gameSize = Parameters.gameSize;

        ResultManager.gameAmount = GameData.character1GameCount + GameData.character2GameCount;

        ResultManager.character1ScoreResult = GameData.character1ScoreResult;
        ResultManager.character2ScoreResult = GameData.character2ScoreResult;

        ResultManager.character1GameCount = GameData.character1GameCount;
        ResultManager.character2GameCount = GameData.character2GameCount;

        ResultManager.previousSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene("ResultScene");
    }
}
