using UnityEngine;

using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;


[System.Serializable] 
public class SavedData
{
    public int CurrentLevelNumber;
    public int Coins;
    public int NewSkinPercent;
    public int NewSkinNumber;
    //public int Level;
    public bool[] OpenHeadSkin = new bool[17];
    public bool[] OpenToiletSkin = new bool[17];
    public int[] ToiletSkinCost = new int[17];
    public int currentHeadIndex;
    public int currentToiletIndex;
    public int TEST;
}

public class SaveManager: MonoBehaviour
{
    [SerializeField] private int[] _TOILETSKINCOST = new int[17];
    
    //public SavedData SavedData;
    //public static SaveManager Instance;
    private void Awake()
    {
        LoadState(() => { });
        DontDestroyOnLoad(this);
        //Instance = this;
        Debug.Log("--------LOAD STATE DATA----------------");
        // Debug.Log(SaveManager.Instance.SavedData.CurrentLevelNumber);
        // Debug.Log(SaveManager.Instance.SavedData.Coins);
        // Debug.Log(SaveManager.Instance.SavedData.currentHeadIndex);
        // Debug.Log(SaveManager.Instance.SavedData.OpenHeadSkin[0]);
        // Debug.Log(SaveManager.Instance.SavedData.OpenHeadSkin[7]);
        Debug.Log("--------STATE DATA LOADED----------------");
    }

    private void Start()
    {
        //Analytics
        GameAnalytics.Initialize();

    }

    public static SavedData CurrentState { get; private set; }
    private void LoadState(Action onLoadCompleted)
    {
        YaSDK.GetData<SavedData>(data =>
        {
            CurrentState = data ??= new SavedData
            {
                CurrentLevelNumber = 0,
                Coins = 0,
                NewSkinPercent = 0,
                NewSkinNumber = 0,
                currentHeadIndex = 0,
                currentToiletIndex = 0
            };

            
            // _generalScore = PlayerInfo.score;
            // scoreText.text = _generalScore.ToString();
            onLoadCompleted();
            // if(CurrentState.CurrentLevelNumber != 0)
            SceneManager.LoadScene(CurrentState.CurrentLevelNumber + 1);
    

        });

    }

    public static void SaveState()
    {
        if (Application.isEditor) return;
            Debug.Log("SaveManager: request to save state has been sent");


        // string jsonString = JsonUtility.ToJson(PlayerInfo);
        CurrentState.CurrentLevelNumber = SceneManager.GetActiveScene().buildIndex;
        YaSDK.SetData(CurrentState);
        // YaSDK.SetToLeaderboard(PlayerInfo.score);
    }
}

