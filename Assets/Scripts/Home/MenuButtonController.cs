using UnityEngine;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    [System.NonSerialized]
    public bool isExpanded;

    [System.NonSerialized]
    public float magnification = 1.30f;

    private float scale = 1.0f;
    private readonly float scaleDelta = 3.0f;

    private Button myButton;

    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        // 自身がinteractable出ない場合は、拡大縮小処理を行わない
        if (!myButton.interactable) { return; }

        if (isExpanded)
        {
            if (scale + scaleDelta * Time.deltaTime < magnification)
            {
                scale += scaleDelta * Time.deltaTime;
                transform.localScale = new Vector3(scale, scale, 0.0f);
            }
        }
        else
        {
            if (scale - scaleDelta * Time.deltaTime > 1.0f)
            {
                scale -= scaleDelta * Time.deltaTime;
                transform.localScale = new Vector3(scale, scale, 0.0f);
            }
        }
    }

    public void Expansion()
    {
        isExpanded = true;
        animator.SetBool("Active", isExpanded);
    }

    public void Reduction()
    {
        isExpanded = false;
        animator.SetBool("Active", isExpanded);
    }
}
