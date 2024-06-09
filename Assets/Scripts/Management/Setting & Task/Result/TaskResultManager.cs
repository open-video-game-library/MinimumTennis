using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskResultManager : MonoBehaviour
{
    public static int taskScore;

    [SerializeField]
    private TMP_Text scoreText;
    private int countingScore;

    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Button retryButton;

    // Start is called before the first frame update
    void Start()
    {
        homeButton.interactable = false;
        retryButton.interactable = false;
    }

    public IEnumerator CountUpScore()
    {
        while (countingScore < taskScore)
        {
            countingScore++;
            scoreText.text = countingScore.ToString();
            yield return new WaitForSeconds(0.050f); // ‘¬“x’²®
        }
        scoreText.color = new Color(1.0f, 1.0f, 0.0f);

        yield return new WaitForSeconds(1.0f);
        homeButton.interactable = true;
        retryButton.interactable = true;
    }

    public void BackHome()
    {
        // Load home scene
        SceneManager.LoadScene("HomeScene");
    }

    public void RestartGame()
    {
        // Reload this scene
        SceneManager.LoadScene("TaskScene");
    }
}
