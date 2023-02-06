using UnityEngine;
using System.Runtime.InteropServices;

public class CSVDataManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void addData(string jsonData);

    [DllImport("__Internal")]
    private static extern void downloadData();
#endif

    [System.Serializable]
    public class Data
    {
        public string winner;
        public int character1GameCount;
        public int character2GameCount;
        public int character1NetCount;
        public int character2NetCount;
        public int character1OutCount;
        public int character2OutCount;
        public int character1TwoBoundCount;
        public int character2TwoBoundCount;
        public int character1DoubleFaultCount;
        public int character2DoubleFaultCount;
        public int maxRallyCount;
    }

    public void PostData(string winner, int character1GameCount, int character2GameCount, int character1NetCount, int character2NetCount,
                         int character1OutCount, int character2OutCount, int character1TwoBoundCount, int character2TwoBoundCount,
                         int character1DoubleFaultCount, int character2DoubleFaultCount, int maxRallyCount)
    {
        // Generate class
        Data data = new Data
        {
            winner = winner,
            character1GameCount = character1GameCount,
            character2GameCount = character2GameCount,
            character1NetCount = character1NetCount,
            character2NetCount = character2NetCount,
            character1OutCount = character1OutCount,
            character2OutCount = character2OutCount,
            character1TwoBoundCount = character1TwoBoundCount,
            character2TwoBoundCount = character2TwoBoundCount,
            character1DoubleFaultCount = character1DoubleFaultCount,
            character2DoubleFaultCount = character2DoubleFaultCount,
            maxRallyCount = maxRallyCount
        };

        // convert to json format and pass to js
        string json = JsonUtility.ToJson(data);
#if UNITY_WEBGL && !UNITY_EDITOR
        addData(json);
#endif
    }

    public void GetData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        downloadData();
#endif
    }
}