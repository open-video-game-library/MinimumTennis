using UnityEngine;

public class CSVDataSetter : MonoBehaviour
{
    [System.NonSerialized]
    public string winner;
    [System.NonSerialized]
    public int character1GameCount;
    [System.NonSerialized]
    public int character2GameCount;
    [System.NonSerialized]
    public int character1NetCount;
    [System.NonSerialized]
    public int character2NetCount;
    [System.NonSerialized]
    public int character1OutCount;
    [System.NonSerialized]
    public int character2OutCount;
    [System.NonSerialized]
    public int character1TwoBoundCount;
    [System.NonSerialized]
    public int character2TwoBoundCount;
    [System.NonSerialized]
    public int character1DoubleFaultCount;
    [System.NonSerialized]
    public int character2DoubleFaultCount;
    [System.NonSerialized]
    public int maxRallyCount;

    private CSVDataManager data;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<CSVDataManager>();
    }

    public void SetCSVData()
    {
        data.PostData(winner, character1GameCount, character2GameCount,
            character1NetCount, character2NetCount, character1OutCount, character2OutCount,
            character1TwoBoundCount, character2TwoBoundCount, character1DoubleFaultCount, character2DoubleFaultCount,
            maxRallyCount);
    }
}
