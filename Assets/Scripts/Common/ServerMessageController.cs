using UnityEngine;

public class ServerMessageController : MonoBehaviour
{
    [SerializeField]
    private GameObject character1ServerMessage;
    [SerializeField]
    private GameObject character2ServerMessage;

    private bool tossed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!tossed) { tossed = GameManager.isToss; }
        else if (!GameManager.inPlay) { tossed = false; }

        if (GameManager.inPlay && !tossed)
        {
            character1ServerMessage.SetActive(GameManager.server == GameManager.character1Name);
            character2ServerMessage.SetActive(GameManager.server == GameManager.character2Name);
        }
        else
        {
            character1ServerMessage.SetActive(false);
            character2ServerMessage.SetActive(false);
        }
        
    }

    void LateUpdate()
    {
        character1ServerMessage.transform.rotation = Camera.main.transform.rotation;
        character2ServerMessage.transform.rotation = Camera.main.transform.rotation;
    }
}
