    using System.Collections;
using System.Collections.Generic;
    using GameAnalyticsSDK;
    using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIScript : Sounds
{
    [SerializeField] public Sprite soundOn;
    [SerializeField] public Sprite soundOff;
    private GameObject mainPanel;
    private GameObject victoryPanel;
    private GameObject failPanel;
    private bool isSoundOn = true;

    public TextMeshProUGUI levelNumber;
    public TextMeshProUGUI levelNumberFail;
    public TextMeshProUGUI levelNumberVictory;

    public Button skipButton;
    // public Button nextButton;
    public Button skipFailButton;
    public Button soundButton;
    public Button shopButton;
    public Button closeShopButton;

    public Button giveSkinButton;

    public Button noButton;
    public Button claimButton;

    public GameObject moneyReward;

    [SerializeField] GameObject OW;
    private OscillatingWheel _OWScript;


    public int coins;

    public GameObject shopPanel;
    private void Start()
    {

        Debug.Log("try load UIScript");
        _OWScript = OW.GetComponent<OscillatingWheel>();
        Debug.Log(1);
        isSoundOn = !AudioListener.pause;
        Debug.Log(2);
        SoundButton();
        Debug.Log(3);
        SoundButton();
        Debug.Log(4);
        //Find лучше не юзать, очень производительно затратная 
        mainPanel = transform.Find("MainPanel").gameObject;
        Debug.Log(5);
        victoryPanel = transform.Find("FinishPanel").Find("VictoryPanel").gameObject;
        Debug.Log(6);
        failPanel = transform.Find("FinishPanel").Find("FailPanel").gameObject;
        Debug.Log(7);
        Debug.Log(8);
        mainPanel.SetActive(true);
        
        skipButton.onClick.AddListener(SkipLevelButton);
        skipFailButton.onClick.AddListener(SkipLevelButton);
        // nextButton.onClick.AddListener(NextLevelButton);
        soundButton.onClick.AddListener(SoundButton);
        shopButton.onClick.AddListener(OpenShopButton);
        closeShopButton.onClick.AddListener(CloseShopButton);
        noButton.onClick.AddListener(noThanksButton);
        giveSkinButton.onClick.AddListener(GiveSkinButton);
        claimButton.onClick.AddListener(ClaimButton);

        Debug.Log(9);
        levelNumber.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        levelNumberFail.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        levelNumberVictory.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        Debug.Log(10);
        noButton.gameObject.SetActive(false);
        giveSkinButton.gameObject.SetActive(false);

        Debug.Log(this.GetType().Name + " is started " + this.name);

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " +  SceneManager.GetActiveScene().buildIndex);

    }

    private void GiveSkinButton()
    {
        SaveManager.CurrentState.currentHeadIndex = SaveManager.CurrentState.NewSkinNumber - 1;
        // SaveManager.CurrentState.currentToiletIndex = SaveManager.CurrentState.currentHeadIndex;

        SaveManager.CurrentState.OpenHeadSkin[SaveManager.CurrentState.currentHeadIndex] = true;
        // SaveManager.CurrentState.OpenToiletSkin[SaveManager.CurrentState.currentHeadIndex] = true;
        
#if UNITY_EDITOR
        NextLevel(0);
#else
        YaSDK.ShowRewardedVideo(onClose: () =>
        {
            if (YaSDK._isRewarded)
                NextLevel(0);
        });
#endif

        //NextLevelWithoutCash();
    }
    private void unHideNoButton()
    {
        noButton.gameObject.SetActive(true);
    }
    public void SoundButton()
    {
       // Debug.Log(AudioListener.volume);
       // Debug.Log(isSoundOn);
        if (AudioListener.volume == 0)
        {
            isSoundOn = false;
        }
        else if (AudioListener.volume == 1f)
        {
            isSoundOn = true;
        }
        UnityEngine.UI.Image btn = GameObject.Find("SoundButton").GetComponent<UnityEngine.UI.Image>();
        isSoundOn = !isSoundOn;
        
        if (isSoundOn)
        {
            AudioListener.volume = 1f;
            btn.sprite = soundOn;
        }
        else
        {
            AudioListener.volume = 0;
            btn.sprite = soundOff;
        }
            
    }
    public void restartLevel()
    {
        //PlaySound(sounds[0]);
        YaSDK.ShowFullscreenAdv(onClose: () =>
        {
            var SceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(SceneIndex);
        });
    }

    public void ClaimButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;

#if UNITY_EDITOR
        NextLevel(200 * _OWScript.mult);
#else
        YaSDK.ShowRewardedVideo(onClose: () =>
        {
            if (YaSDK._isRewarded)
                NextLevel(200 * _OWScript.mult);
        });
#endif
        //NextLevel();

    }
    public void noThanksButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // SaveManager.Instance.SavedData.Coins += 100;
        NextLevel(0);

    }
    public void OpenShopButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        Debug.Log("OPEN SHOP BUTTON ____________________________________-");
        if (shopPanel)
        {
            Debug.Log("shop panel opened");
        }
        else
        {
            Debug.Log("No game object called wibble found");
        }
        Debug.Log("Shop panel " + shopPanel.gameObject.name);
        shopPanel.SetActive(true);
        Debug.Log("Shop panel " + shopPanel.gameObject.tag);
        mainPanel.SetActive(false);

    }
    public void CloseShopButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        restartLevel();
       // shopPanel.SetActive(false);
       // mainPanel.SetActive(true);
    }
    private void NextLevel(int moneyReward)
    {
        SaveManager.CurrentState.Coins += moneyReward - 100;
        SaveManager.SaveState();
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(SceneManager.sceneCountInBuildSettings - 1 == sceneIndex)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
    }
    private void hideMainPanel()
    {
        mainPanel.SetActive(false);
    }
    public void win()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + SceneManager.GetActiveScene().buildIndex);

        SaveManager.CurrentState.Coins += 100;
        SaveManager.SaveState();
        victoryPanel.SetActive(true);
        hideMainPanel();
        if(SaveManager.CurrentState.NewSkinPercent >= 80)
        {
            moneyReward.gameObject.SetActive(false);
            // nextButton.gameObject.SetActive(false);
            giveSkinButton.gameObject.SetActive(true);
            OW.SetActive(false);
        }
        Invoke("unHideNoButton", 4);


    }
    public void lose()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level " + SceneManager.GetActiveScene().buildIndex);
        hideMainPanel();
        failPanel.SetActive(true);
    }

    public void SkipLevelButton()
    {
#if UNITY_EDITOR
        NextLevel(0);
#else
        YaSDK.ShowRewardedVideo(onClose: () =>
        {
            if (YaSDK._isRewarded) 
                NextLevel(0);
        });
#endif

    }
    
        
}
