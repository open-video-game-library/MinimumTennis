using UnityEngine;

public class ReplayObjectController : MonoBehaviour
{
    public AnimationCurve positionX = new();
    public AnimationCurve positionY = new();
    public AnimationCurve positionZ = new();

    public AnimationCurve quaternionX = new();
    public AnimationCurve quaternionY = new();
    public AnimationCurve quaternionZ = new();
    public AnimationCurve quaternionW = new();

    public AnimationRecorder animationRecorder;

    public void RecordTransform(float worldTime)
    {
        // é©ï™é©êgÇÃà íuÇ∆âÒì]ÇãLò^Ç∑ÇÈ
        Add(transform.position, transform.rotation, worldTime);
    }

    public void Add(Vector3 pos, Quaternion v, float worldTime)
    {
        positionX.AddKey(worldTime, pos.x);
        positionY.AddKey(worldTime, pos.y);
        positionZ.AddKey(worldTime, pos.z);

        quaternionX.AddKey(worldTime, v.x);
        quaternionY.AddKey(worldTime, v.y);
        quaternionZ.AddKey(worldTime, v.z);
        quaternionW.AddKey(worldTime, v.w);
    }

    public void Replay(float localTime)
    {
        transform.position = new Vector3(positionX.Evaluate(localTime), positionY.Evaluate(localTime), positionZ.Evaluate(localTime));
        transform.rotation = new Quaternion(quaternionX.Evaluate(localTime), quaternionY.Evaluate(localTime), quaternionZ.Evaluate(localTime), quaternionW.Evaluate(localTime));
        animationRecorder.AnimatorUpdate();
    }
}
