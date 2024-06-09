using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAudioManager : MonoBehaviour
{
    private static AudioSource audioSource;

    [SerializeField]
    private AudioClip successSound;
    private static AudioClip success;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        success = successSound;
    }

    public static void PlaySuccessSound()
    {
        // 得点が入る際の効果音を鳴らす（TaskBallController側からこの関数を呼ぶため、staticにする）
        audioSource.PlayOneShot(success);
    }
}
