using UnityEngine;

public class AvatarSkeletonGenerator : MonoBehaviour
{
    public GameObject[] vertices = new GameObject[3];
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = vertices.Length;
    }

    void Update()
    {
        int idx = 0;
        foreach (GameObject v in vertices)
        {
            line.SetPosition(idx, v.transform.position);
            idx++;
        }
    }
}
