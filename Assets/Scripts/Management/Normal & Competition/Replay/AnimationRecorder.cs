using UnityEngine;

public class AnimationRecorder : MonoBehaviour
{
    private bool isPlayback;

    [System.NonSerialized]
    public Animator animator;
    private Animator recorderAnimator;

    // Start is called before the first frame update
    void Start()
    {
        recorderAnimator = GetComponent<Animator>();
    }

    public void StartRecord()
    {
        if (isPlayback)
        {
            // リプレイの再生中に、再度記録が開始された場合は、再生中のリプレイを中止する
            animator.StopPlayback();
            recorderAnimator.StopPlayback();
        }

        animator.StartRecording(0);
        recorderAnimator.StartRecording(0);
        isPlayback = false;
    }

    public void StopRecord()
    {
        isPlayback = false;
        animator.StopRecording();
        recorderAnimator.StopRecording();
    }

    public void StartPlayback(float replayTime)
    {
        if (animator.recorderStopTime <= 0) { return; }
        if (animator && recorderAnimator)
        {
            animator.Rebind();
            recorderAnimator.Rebind();

            animator.StartPlayback();
            recorderAnimator.StartPlayback();

            animator.playbackTime = Mathf.Max(animator.recorderStopTime - replayTime, 0.0f);
            recorderAnimator.playbackTime = Mathf.Max(animator.playbackTime, 0.0f);

            isPlayback = true;
        }
    }

    public void StopPlayback()
    {
        isPlayback = false;

        animator.Rebind();
        recorderAnimator.Rebind();
        animator.StopPlayback();
        recorderAnimator.StopPlayback();
    }

    public bool AnimatorUpdate()
    {
        bool isAnimationEnded = false;

        if (isPlayback)
        {
            float playBackTime = recorderAnimator.playbackTime + Time.deltaTime;

            if (playBackTime > recorderAnimator.recorderStopTime)
            {
                // 記録されたアニメーションの再生が終わったら
                isAnimationEnded = true;
                Debug.Log("アニメーションの再生が終わりました。");
                StopPlayback();
            }

            if (isPlayback) { animator.playbackTime = playBackTime; }
            if (isPlayback) { recorderAnimator.playbackTime = playBackTime; }
        }

        return isAnimationEnded;
    }
}
