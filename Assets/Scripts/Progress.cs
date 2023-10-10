using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;



[System.Serializable]
public class PlayerInfo
{
    public int Coins;
    public int NewSkinPercent;
    public int NewSkinNumber;
    public int Level;
}

public class Progress : MonoBehaviour
{
    public PlayerInfo PlayerInfo;
    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    [SerializeField] TextMeshProUGUI _playerInfoText;
    public static Progress Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
#if  !UNITY_EDITOR && UNITY_WEBGL____
            LoadExtern();
#endif

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Save()
    {
        string jsonString = JsonUtility.ToJson(PlayerInfo);
#if  !UNITY_EDITOR && UNITY_WEBGL____
        SaveExtern(jsonString);
#endif
    }

    public void SetPlayerInfo(string value)
    {
        PlayerInfo = JsonUtility.FromJson<PlayerInfo>(value);
    }
}
