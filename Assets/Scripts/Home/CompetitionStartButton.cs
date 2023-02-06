using UnityEngine;
using UnityEngine.SceneManagement;

public class CompetitionStartButton : MonoBehaviour
{
    public void OnClickCompetitionButton()
    {
        SceneManager.LoadScene("CompetitionScene");
    }
}
