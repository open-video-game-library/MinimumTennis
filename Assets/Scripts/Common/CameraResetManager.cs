using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraResetManager : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    public void Teleport(Vector3 defaultPosition)
    {
        transposer.m_XDamping = 0.0f;
        transposer.m_YDamping = 0.0f;
        transposer.m_ZDamping = 0.0f;
        transform.position = defaultPosition;

        // 次のフレームで元に戻す
        StartCoroutine(ResetBrainUpdateMethod());
    }

    private IEnumerator ResetBrainUpdateMethod()
    {
        // 1フレーム待つ
        for (int i = 0; i < 10; i++) { yield return null; }

        // 元に戻す
        transposer.m_XDamping = 1.0f;
        transposer.m_YDamping = 1.0f;
        transposer.m_ZDamping = 1.0f;
    }
}
