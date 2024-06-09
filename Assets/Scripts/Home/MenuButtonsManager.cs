using UnityEngine;

public class MenuButtonsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] buttons;
    private MenuButtonController[] buttonControllers;
    private RectTransform[] buttonRectTransforms;

    // Start is called before the first frame update
    void Start()
    {
        buttonControllers = new MenuButtonController[buttons.Length];
        buttonRectTransforms = new RectTransform[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttonControllers[i] = buttons[i].GetComponent<MenuButtonController>();
            buttonRectTransforms[i] = buttons[i].GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Vector2 localMousePosition = buttonRectTransforms[i].InverseTransformPoint(Input.mousePosition);

            // Buttonの範囲内かどうかチェック
            if (buttonRectTransforms[i].rect.Contains(localMousePosition))
            {
                if (buttonControllers[i].isExpanded) { continue; }

                for (int j = 0; j < buttons.Length; j++) { buttonControllers[j].Reduction(); }
                buttonControllers[i].Expansion();
            }
            else { buttonControllers[i].Reduction(); }
        }
    }
}
