using UnityEngine;
using Cinemachine;

public class MainCharacterSetter : MonoBehaviour
{
    private GameObject mainCharacter;

    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Awake()
    {
        mainCharacter = CharacterGenerator.GetCharacter(0);
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = mainCharacter.transform;
        virtualCamera.LookAt = mainCharacter.transform;
    }
}
