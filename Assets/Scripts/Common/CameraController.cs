using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject character;

    private Vector3 offset;
    private float characterX;
    private float characterZ;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - character.transform.position;
        characterX = character.transform.position.x;
        characterZ = character.transform.position.z;
        transform.Translate(transform.position.x - characterX, 0.0f, transform.position.z - characterZ);
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, character.transform.position + offset, 5.0f * Time.deltaTime);
    }
}
