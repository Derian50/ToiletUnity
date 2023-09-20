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
    private bool isSoundOn = true;

    public TextMeshProUGUI levelNumber;
    public TextMeshProUGUI levelNumberFail;
    public TextMeshProUGUI levelNumberVictory;

    public Button skipButton;
    public Button nextButton;
    public Button skipFailButton;
    public Button soundButton;

    private void Start()
    {
        isSoundOn = !AudioListener.pause;
        SoundButton();
        SoundButton();
        //Find лучше не юзать, очень производительно затратная 
        mainPanel = transform.Find("MainPanel").gameObject;
        victoryPanel = transform.Find("FinishPanel").Find("VictoryPanel").gameObject;
        failPanel = transform.Find("FinishPanel").Find("FailPanel").gameObject;
        mainPanel.SetActive(true);
        
        skipButton.onClick.AddListener(SkipLevelButton);
        skipFailButton.onClick.AddListener(SkipLevelButton);
        nextButton.onClick.AddListener(NextLevelButton);
        soundButton.onClick.AddListener(SoundButton);

        levelNumber.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        levelNumberFail.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        levelNumberVictory.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        
        
        


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
            AudioListener.volume = 1.0f;
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

            NextLevel();
            
    }

    private void NextLevel()
    {
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
    private void hideMainPanel()
    {
        mainPanel.SetActive(false);
    }
    public void win()
    {
        hideMainPanel();
        victoryPanel.SetActive(true);
    }
    public void lose()
    {
        hideMainPanel();
        failPanel.SetActive(true);
    }

    public void SkipLevelButton()
    {
        YaSDK.ShowRewardedVideo( onRewarded: NextLevel);
    }
    
        
}
