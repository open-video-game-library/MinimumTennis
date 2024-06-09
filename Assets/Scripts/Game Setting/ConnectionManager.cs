using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField]
    private ControllerSelector player1AvailableController;
    [SerializeField]
    private ControllerSelector player2AvailableController;

    [SerializeField]
    private Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.interactable = false;
    }

    void Update()
    {
        // 1Pと2Pの両者のコントローラが正しく選択された場合、スタートボタンを押せるようにする
        startButton.interactable = player1AvailableController.ManageSelection() && player2AvailableController.ManageSelection();
    }
}
