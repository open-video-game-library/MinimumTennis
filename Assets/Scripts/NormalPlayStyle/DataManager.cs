using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DataManager : MonoBehaviour
{
    // If it is a WebGL version, get the function that works on the JavaScript side
    // "addData" Å® function to send data to js for each attempt
    // "downloadData" Å® Function to download csv data after any attempt
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void addData(string jsonData);

    [DllImport("__Internal")]
    private static extern void downloadData();
#endif

    // Define the class of data to be acquired
    [System.Serializable]
    public class Data
    {
        public string gameResult; // victory or defeat
        public int playerGameNum; // Number of games won by the player
        public int enemyGameNum; // Number of games won by opponent
        public int netNum; // Number of times the player has netted
        public int outNum; // Number of times the player has been out
        public int twoBoundNum; // Number of times the player's 2 bounces
        public int doubleFaultNum; // Number of times the player has double-faulted
        public int maxRallyCount; // Maximum number of rallies
    }

    // Function to be called when the trial is over
    public void postData(string _gameResult, int _playerGameNum, int _enemyGameNum, int _netNum, int _outNum,
                         int _twoBoundNum, int _doubleFaultNum, int _maxRallyCount)
    {
        // Generate class
        Data data = new Data(); 

        data.gameResult = _gameResult;
        data.playerGameNum = _playerGameNum;
        data.enemyGameNum = _enemyGameNum;
        data.netNum = _netNum;
        data.outNum = _outNum;
        data.twoBoundNum = _twoBoundNum;
        data.doubleFaultNum = _doubleFaultNum;
        data.maxRallyCount = _maxRallyCount;

        // convert to json format and pass to js
        string json = JsonUtility.ToJson(data);
#if UNITY_WEBGL && !UNITY_EDITOR
        addData(json);
#endif
    }

    // Function to call when the download button is pressed
    public void getData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        downloadData();
#endif
    }
}