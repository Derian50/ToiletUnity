    using System.Collections;
using System.Collections.Generic;
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
    private GameObject shopPanel;
    private bool isSoundOn = true;

    public TextMeshProUGUI levelNumber;
    public TextMeshProUGUI levelNumberFail;
    public TextMeshProUGUI levelNumberVictory;

    public Button skipButton;
    public Button nextButton;
    public Button skipFailButton;
    public Button soundButton;
    public Button shopButton;
    public Button closeShopButton;

    public Button giveSkinButton;

    public Button noButton;

    public GameObject moneyReward;

    [SerializeField] GameObject OW;
    private OscillatingWheel _OWScript;

    private void Start()
    {
        _OWScript = OW.GetComponent<OscillatingWheel>();
        isSoundOn = !AudioListener.pause;
        SoundButton();
        SoundButton();
        //Find лучше не юзать, очень производительно затратная 
        mainPanel = transform.Find("MainPanel").gameObject;
        victoryPanel = transform.Find("FinishPanel").Find("VictoryPanel").gameObject;
        failPanel = transform.Find("FinishPanel").Find("FailPanel").gameObject;
        shopPanel = transform.Find("ShopScreen").gameObject;
        mainPanel.SetActive(true);
        
        skipButton.onClick.AddListener(SkipLevelButton);
        skipFailButton.onClick.AddListener(SkipLevelButton);
        nextButton.onClick.AddListener(NextLevelButton);
        soundButton.onClick.AddListener(SoundButton);
        shopButton.onClick.AddListener(OpenShopButton);
        closeShopButton.onClick.AddListener(CloseShopButton);
        noButton.onClick.AddListener(noThanksButton);
        giveSkinButton.onClick.AddListener(GiveSkinButton);

        levelNumber.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        levelNumberFail.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        levelNumberVictory.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();

        noButton.gameObject.SetActive(false);
        giveSkinButton.gameObject.SetActive(false);




    }

    private void GiveSkinButton()
    {
        Progress.Instance.PlayerInfo.currentHeadIndex = Progress.Instance.PlayerInfo.NewSkinNumber;
        Progress.Instance.PlayerInfo.OpenHeadSkin[Progress.Instance.PlayerInfo.currentHeadIndex] = true;
        YaSDK.ShowRewardedVideo(onClose: () =>
        {
            if (YaSDK._isRewarded)
                NextLevelWithoutCash();
        });
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
        var SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(SceneIndex);
    }

    public void NextLevelButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        YaSDK.ShowRewardedVideo(onClose: () =>
        {
            if (YaSDK._isRewarded)
                NextLevel();
        });
        //NextLevel();

    }
    public void noThanksButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Progress.Instance.PlayerInfo.Coins += 100;
        NextLevelWithoutCash();

    }
    public void OpenShopButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        shopPanel.SetActive(true);
        mainPanel.SetActive(false);

    }
    public void CloseShopButton()
    {
        // var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        shopPanel.SetActive(false);
        mainPanel.SetActive(true);

    }
    private void NextLevel()
    {
        Progress.Instance.PlayerInfo.Coins += (200 * _OWScript.mult);
        Progress.Instance.Save();
        SaveManager.SaveState();
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(SceneManager.sceneCountInBuildSettings - 1 == sceneIndex)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
    }
    private void NextLevelWithoutCash()
    {
        
        Progress.Instance.Save();
        SaveManager.SaveState();
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.sceneCountInBuildSettings - 1 == sceneIndex)
        {
            SceneManager.LoadScene(0);
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

        victoryPanel.SetActive(true);
        hideMainPanel();
        if(Progress.Instance.PlayerInfo.NewSkinPercent >= 80)
        {
            moneyReward.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            giveSkinButton.gameObject.SetActive(true);
            OW.SetActive(false);
        }
        Invoke("unHideNoButton", 4);
        
    }
    public void lose()
    {
        hideMainPanel();
        failPanel.SetActive(true);
    }

    public void SkipLevelButton()
    {
        YaSDK.ShowRewardedVideo(onClose: () =>
        {
            if (YaSDK._isRewarded) 
                NextLevelWithoutCash();
        });
    }
    
        
}
