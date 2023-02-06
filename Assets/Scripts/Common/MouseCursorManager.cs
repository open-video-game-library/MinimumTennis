using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    private Vector3 cursorPreviousPosition;
    private float hiddenTime;

    // Update is called once per frame
    void Update()
    {
        ManageCursor();
    }

    private void ManageCursor()
    {
        Vector3 cursorCurrentPosition = Input.mousePosition;

        if (cursorCurrentPosition != cursorPreviousPosition || Input.GetMouseButtonDown(0))
        {
            Cursor.visible = true;
            hiddenTime = 0.0f;
        }
        else
        {
            if (hiddenTime > 2.0f) { Cursor.visible = false; }
            else { hiddenTime += Time.deltaTime; }
        }

        cursorPreviousPosition = cursorCurrentPosition;
    }
}
