using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void OnClickHomeButton()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
