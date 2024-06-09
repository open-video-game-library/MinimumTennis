using UnityEngine;
using Cinemachine;

public class MultipleCharacterSetter : MonoBehaviour
{
    [SerializeField]
    private Players players;

    private GameObject character;

    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        character = CharacterGenerator.GetCharacter((int)players);
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = character.transform;
        virtualCamera.LookAt = character.transform;
    }
}
