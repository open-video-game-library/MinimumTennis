using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject selectCamera;

    [SerializeField]
    private Button leftArrow;
    [SerializeField]
    private Button rightArrow;

    private readonly Vector3 defalutCameraPosition = new Vector3(0.0f, 3.5f, 0.0f);
    
    [System.NonSerialized]
    public int currentNum = 0;
    private int characterMaxNum;

    public void SelectFirstCharacter(int characterNum, float span)
    {
        characterMaxNum = characterNum;

        leftArrow.gameObject.SetActive(currentNum != 0);
        rightArrow.gameObject.SetActive(currentNum != characterNum - 1);

        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentNum > 0) { currentNum--; }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentNum < characterNum - 1) { currentNum++; }

        Vector3 targetPosition = new Vector3(defalutCameraPosition.x + currentNum * span, defalutCameraPosition.y, defalutCameraPosition.z);
        selectCamera.transform.position = Vector3.MoveTowards(selectCamera.transform.position, targetPosition, 100.0f * Time.deltaTime);
    }

    public void SelectSecondCharacter(int characterNum, float span)
    {
        characterMaxNum = characterNum;

        leftArrow.gameObject.SetActive(currentNum != 0);
        rightArrow.gameObject.SetActive(currentNum != characterNum - 1);

        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentNum > 0) { currentNum--; }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentNum < characterNum - 1) { currentNum++; }

        Vector3 targetPosition = new Vector3(defalutCameraPosition.x + currentNum * span, defalutCameraPosition.y, defalutCameraPosition.z + 60.0f);
        selectCamera.transform.position = Vector3.MoveTowards(selectCamera.transform.position, targetPosition, 100.0f * Time.deltaTime);
    }

    public void ViewLeftCharacter()
    {
        if (currentNum > 0) { currentNum--; }
    }

    public void ViewRightCharacter()
    {
        if (currentNum < characterMaxNum - 1) { currentNum++; }
    }
}
