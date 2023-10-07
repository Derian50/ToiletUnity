using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class YaSDK : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAd();

    [DllImport("__Internal")]
    private static extern void ShowReward();

    [DllImport("__Internal")]
    private static extern void SetPlayerData(string data);

    [DllImport("__Internal")]
    private static extern void GetPlayerData();

    [DllImport("__Internal")]
    private static extern string GetLang();

    private static bool _isAd;
    
    private static bool _adOpened = false;


    #region AUDIO

    private static void SetAudioOn(bool isOn)
    {
        // isOn &= !_isAd;
        AudioListener.pause = !isOn;
        // AudioListener.volume = isOn ? 1 : 0;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if(_adOpened)
            return;

        AudioListener.pause = (hasFocus == false);
    }

    private void OnApplicationPause(bool isPaused)
    {
        if(_adOpened)
            return;
        // Debug.Log("Paused" + isPaused);

        AudioListener.pause = isPaused;
    }

    #endregion

    #region FULLSCREEN

    private static Action _onOpenAdv;
    private static Action _onCloseAdv;
    private static Action _onErrorAdv;
    private static Action _onOfflineAdv;

    public void OnOpen()
    {
        _adOpened = true;

        // _isAd = true;
        SetAudioOn(false);
        _onOpenAdv?.Invoke();
    }

    public void OnClose()
    {
        _adOpened = false;

        // _isAd = false;
        SetAudioOn(true);
        _onCloseAdv?.Invoke();
    }

    public void OnError()
    {
        _onErrorAdv?.Invoke();
    }

    public void OnOffline()
    {
        // _adOpened = false;

        // _isAd = false;
        // SetAudioOn(true);
        _onOfflineAdv?.Invoke();
    }

    public static void ShowFullscreenAdv(Action onOpen = null, Action onClose = null, Action onError = null,
        Action onOffline = null)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        onOpen?.Invoke();
        onClose?.Invoke();
        onError?.Invoke();
        onOffline?.Invoke();
#else
        _onOpenAdv = onOpen;
        _onCloseAdv = onClose;
        _onErrorAdv = onError;
        _onOfflineAdv = onOffline;
        ShowAd();
#endif
    }

    #endregion

    #region REWARD

    private static Action _onOpenReward;
    private static Action _onRewarded;
    private static Action _onCloseReward;
    private static Action _onErrorReward;

    public void OnOpenReward()
    {
        _isRewarded = false;

        _adOpened = true;
        Time.timeScale = 0f;
        // _isAd = true;
        SetAudioOn(false);
        _onOpenReward?.Invoke();
    }

    public static bool _isRewarded;
    public void OnRewarded()
    {
        _isRewarded = true;
        _onRewarded?.Invoke();
    }

    public void OnCloseReward()
    {
        _adOpened = false;
        Time.timeScale = 1f;
        // _isAd = false;
        SetAudioOn(true);
        _onCloseReward?.Invoke();
    }

    public void OnErrorReward()
    {
        _isRewarded = false;
        // _adOpened = false;
        // _isAd = false;
        // SetAudioOn(true);
        _onErrorReward?.Invoke();
    }

    public static void ShowRewardedVideo(Action onOpen = null, Action onRewarded = null, Action onClose = null,
        Action onError = null)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        onOpen?.Invoke();
        onRewarded?.Invoke();
        onClose?.Invoke();
        onError?.Invoke();
#else
        _onOpenReward = onOpen;
        _onRewarded = onRewarded;
        _onCloseReward = onClose;
        _onErrorReward = onError;
        ShowReward();
#endif
    }

    #endregion

    #region DATA

    private static Action<string> _onGetData;
#if UNITY_EDITOR || !UNITY_WEBGL
    private static string _data;
#endif

    public void OnGetData(string data)
    {
        _onGetData?.Invoke(data);
    }

    public static void GetData<T>(Action<T> onGetData)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        onGetData?.Invoke(JsonUtility.FromJson<T>(_data));
#else
        _onGetData = s => onGetData?.Invoke(JsonUtility.FromJson<T>(s));
        GetPlayerData();
#endif
    }

    public static void SetData(object objData)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        _data = JsonUtility.ToJson(objData);
        print(JsonUtility.ToJson(objData, true) + "\n");
#else
        SetPlayerData(JsonUtility.ToJson(objData));
#endif
    }

    #endregion

    #region LANGUAGE

    private static string _language;

    public static string GetLanguage()
    {
        if (string.IsNullOrEmpty(_language))
#if UNITY_EDITOR || !UNITY_WEBGL
            _language = "tr";
#else
            _language = GetLang();
#endif
        return _language;
    }

    #endregion
}
