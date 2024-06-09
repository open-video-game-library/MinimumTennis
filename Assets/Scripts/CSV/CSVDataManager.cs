using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System;

public class CSVDataManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void addData(string jsonData);

    [DllImport("__Internal")]
    private static extern void downloadData();
#else

#endif

    // System.IO
    private static StreamWriter resultSW;
    private static readonly List<CSVData> csvList = new List<CSVData>();

    public static void SetData(CSVData csvData)
    {
        // convert to json format and pass to js
        string json = JsonUtility.ToJson(csvData);

#if UNITY_WEBGL && !UNITY_EDITOR
        addData(json);
#else
        AddData(csvData);
#endif
    }

    public static void GetData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        downloadData();
#endif
    }

    public static void CreateCSV()
    {
        DateTime currentTime = DateTime.Now;
        string year = currentTime.Year.ToString();
        string month = currentTime.Month.ToString();
        string day = currentTime.Day.ToString();
        string hour = currentTime.Hour.ToString();
        string minute = currentTime.Minute.ToString();
        string second = currentTime.Second.ToString();

        // SaveDataフォルダが存在しない場合は、新しく作る
        if (!Directory.Exists(Application.dataPath + "/Game Result")) { Directory.CreateDirectory(Application.dataPath + "/Game Result"); }
        resultSW = new StreamWriter(@Application.dataPath + "/Game Result/" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "_result.csv",
            false, Encoding.GetEncoding("UTF-8"));

        // ラベルを書き込む
        string[] labels = { "id", "winner", "character1_game_count", "character2_game_count", "character1_net_count", "character2_net_count",
            "character1_out_count", "character2_out_count", "character1_twobound_count", "character2_twobound_count",
            "character1_doublefault_count", "character2_doublefault_count", "max_rally_count" };

        // 文字列配列のすべての要素を「,」で連結する
        string label = string.Join(",", labels);

        // 「,」で連結した文字列をcsvファイルへ書き込む
        resultSW.WriteLine(label);
    }

    public static void ExportCSV()
    {
        if (csvList[0] == null) { return; }

        CreateCSV();

        for (int i = 0; i < csvList.Count; i++)
        {
            string[] score = { (i + 1).ToString(), csvList[i].winner, csvList[i].character1GameCount.ToString(), csvList[i].character2GameCount.ToString(),
            csvList[i].character1NetCount.ToString(), csvList[i].character2NetCount.ToString(), csvList[i].character1OutCount.ToString(), csvList[i].character2OutCount.ToString(),
            csvList[i].character1TwoBoundCount.ToString(), csvList[i].character2TwoBoundCount.ToString(),
            csvList[i].character1DoubleFaultCount.ToString(), csvList[i].character2DoubleFaultCount.ToString(), csvList[i].maxRallyCount.ToString() };

            string scoreData = string.Join(",", score);

            resultSW.WriteLine(scoreData);
        }

        SaveCSV();
    }

    public static void SaveCSV()
    {
        // CSVファイルへの書き込みを終了
        if (resultSW != null) { resultSW.Close(); }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private static void AddData(CSVData csvData)
    {
        csvList.Add(csvData);
    }
}