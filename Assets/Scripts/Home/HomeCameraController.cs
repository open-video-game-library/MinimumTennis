using UnityEngine;
using Cinemachine;

[System.Serializable]
public class Route
{
    [SerializeField]
    public int startCameraNum;
    [SerializeField]
    public int endCameraNum;

    public Route(int _point1, int _point2)
    {
        startCameraNum = _point1;
        endCameraNum = _point2;
    }

    public bool CheckSameRoute(Route route)
    {
        return startCameraNum == route.startCameraNum && endCameraNum == route.endCameraNum
            || startCameraNum == route.endCameraNum && endCameraNum == route.startCameraNum;
    }
}

public class HomeCameraController : MonoBehaviour
{
    private CinemachineMixingCamera mixingCamera;
    private int cameraAmount;

    [SerializeField]
    private Route[] routes;
    private int routeCount;

    private float cameraWeight;
    private float weightDelta = 0.10f;

    // Start is called before the first frame update
    void Start()
    {
        mixingCamera = GetComponent<CinemachineMixingCamera>();
        cameraAmount = mixingCamera.ChildCameras.Length;
    }

    // Update is called once per frame
    void Update()
    {
        cameraWeight += weightDelta * Time.deltaTime;

        mixingCamera.SetWeight(routes[routeCount].startCameraNum, 1.0f - cameraWeight);
        mixingCamera.SetWeight(routes[routeCount].endCameraNum, cameraWeight);

        if (cameraWeight >= 1.0f)
        {
            routeCount++;
            routeCount %= routes.Length;
            cameraWeight = 0.0f;
            for (int i = 0; i < cameraAmount; i++) { mixingCamera.SetWeight(i, 0.0f); }
        }
    }
}
