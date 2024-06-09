using UnityEngine;

[System.Serializable]
public class TargetArea
{
    public Vector3 leftTopCorner;
    public Vector3 rightUnderCorner;

    public TargetArea(Vector3 _leftTopCorner, Vector3 _rightUnderCorner)
    {
        leftTopCorner = _leftTopCorner;
        rightUnderCorner = _rightUnderCorner;
    }
}

public class TargetAreaManager : MonoBehaviour
{
    [SerializeField]
    private GameObject targetAreaPrefab;
    private static GameObject targetArea;

    [SerializeField]
    private TargetArea[] areaShapes;
    private static TargetArea[] areaShapesCopy;
    private static TargetArea currentTargetArea;
    private static int currentAreaNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        targetArea = Instantiate(targetAreaPrefab);
        areaShapesCopy = areaShapes;
        currentAreaNum = 0;
        ShuffleTargetAreaPattern();
        ChangeTargetArea(DecideNextPosition());
    }

    void Update()
    {
        targetArea.SetActive(TaskData.taskState == TaskState.Playing);
    }

    public static bool CheckInTargetArea(Vector3 arrivalPosition)
    {
        float ballPosX = arrivalPosition.x;
        float ballPosZ = arrivalPosition.z;

        bool horizontalCheck = currentTargetArea.leftTopCorner.x <= ballPosX && ballPosX <= currentTargetArea.rightUnderCorner.x;
        bool verticalCheck = currentTargetArea.rightUnderCorner.z <= ballPosZ && ballPosZ <= currentTargetArea.leftTopCorner.z;

        if (horizontalCheck && verticalCheck) { ChangeTargetArea(DecideNextPosition()); }
        return horizontalCheck && verticalCheck;
    }

    private static void ChangeTargetArea(TargetArea nextAreaConrner)
    {
        // 引数からTargetAreaの形と位置を計算し、適用する処理
        Vector3 leftTopCorner = nextAreaConrner.leftTopCorner;
        Vector3 rigthUnderConner = nextAreaConrner.rightUnderCorner;
        float horizontalLength = rigthUnderConner.x - leftTopCorner.x;
        float verticalLength = leftTopCorner.z - rigthUnderConner.z;

        Vector3 targetAreaPosition = new Vector3(
            (leftTopCorner.x + rigthUnderConner.x) / 2.0f,
            targetArea.transform.position.y, 
            (leftTopCorner.z + rigthUnderConner.z) / 2.0f);

        targetArea.transform.position = targetAreaPosition;
        targetArea.transform.localScale = new Vector3(horizontalLength, verticalLength, 1.0f);

        currentAreaNum++;
        if (currentAreaNum == areaShapesCopy.Length)
        {
            currentAreaNum = 0;
            ShuffleTargetAreaPattern();
        }
    }

    private static TargetArea DecideNextPosition()
    {
        TargetArea nextArea = areaShapesCopy[currentAreaNum];
        currentTargetArea = nextArea;

        return nextArea;
    }

    private static void ShuffleTargetAreaPattern()
    {
        int n = areaShapesCopy.Length;
        TargetArea lastElement = areaShapesCopy[areaShapesCopy.Length - 1];

        for (int i = n - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            TargetArea temp = areaShapesCopy[i];
            areaShapesCopy[i] = areaShapesCopy[r];
            areaShapesCopy[r] = temp;
        }

        while (lastElement == areaShapesCopy[0])
        {
            for (int i = n - 1; i > 0; i--)
            {
                int r = Random.Range(0, i + 1);
                TargetArea temp = areaShapesCopy[i];
                areaShapesCopy[i] = areaShapesCopy[r];
                areaShapesCopy[r] = temp;
            }
        }
    }
}
