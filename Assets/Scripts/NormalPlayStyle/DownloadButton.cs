using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadButton : MonoBehaviour
{
    public GameObject dataManager;
    private DataManager data;

    void Start()
    {
        data = dataManager.GetComponent<DataManager>();
    }

    // If you press the button
    public void OnClickButton()
    {
        data.getData();
    }
}
