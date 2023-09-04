using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[Serializable]
public class SavedData
{
    public int CurrentLevelNumber;
}

public class SaveManager: MonoBehaviour
{
    private void Awake()
    {
        LoadState(() => { });
        DontDestroyOnLoad(this);
    }
    
    public static SavedData CurrentState { get; private set; }
    private void LoadState(Action onLoadCompleted)
    {
        YaSDK.GetData<SavedData>(data =>
        {
            CurrentState = data ??= new SavedData { CurrentLevelNumber = 0};

            
            // _generalScore = PlayerInfo.score;
            // scoreText.text = _generalScore.ToString();
            onLoadCompleted();
            if(CurrentState.CurrentLevelNumber != 0)
                SceneManager.LoadScene(CurrentState.CurrentLevelNumber);

        });

    }
    public void MakeInitialState()
    {
        YaSDK.SetData("0");
        // _runtimeState = _initialState;
    }
    
    // public static void SetState(SavedData savedData)
    // {
    //     YaSDK.SetData(savedData);
    //
    // }
    
    public static void SaveState()
    {
        Debug.Log("SaveManager: request to save state has been sent");

        // string jsonString = JsonUtility.ToJson(PlayerInfo);
        CurrentState.CurrentLevelNumber = SceneManager.GetActiveScene().buildIndex;
        
        YaSDK.SetData(CurrentState);
        // YaSDK.SetToLeaderboard(PlayerInfo.score);
    }


    // public void LoadState(Action onLoadCompleted)
    // {
    //     Debug.Log("Entered YaSaveManager.LoadState");
    //     YaSDK.GetData<SavedState<SerializableRuntimeSavedInt, SerializableRuntimeSavedFloat>>(
    //         state =>
    //         {
    //             Debug.Log($"Load competed. Listing some savestate params. Has state: {state.HasSave}, Stage number: {state.StageNumber}");
    //             // _runtimeState = state;
    //             // BoughtTimes = new Dictionary<MinionType, ISavedInt>
    //             // {
    //             //     { MinionType.Melee, _runtimeState.MeleeBoughtTimes },
    //             //     { MinionType.Ranged, _runtimeState.RangedBoughTimes }
    //             // };
    //             onLoadCompleted();
    //         });
    // }
    

    

    // public static void GetData(Action<SavedData> onGetData)
    // {
    //     YaSDK.GetData<SavedData>(data =>
    //     {
    //         CurrentState = data ??= new SavedData();
    //         onGetData?.Invoke(data);
    //     });
    // }


}

