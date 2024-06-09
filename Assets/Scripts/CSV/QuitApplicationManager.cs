using UnityEngine;

public class QuitApplicationManager : MonoBehaviour
{
    void OnApplicationQuit()
    {
        CSVDataManager.SaveCSV();
    }
}
