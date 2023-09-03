using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIScript : Sounds
{
    [SerializeField] public Sprite soundOn;
    [SerializeField] public Sprite soundOff;
    private GameObject mainPanel;
    private GameObject victoryPanel;
    private GameObject failPanel;
    private bool isSoundOn = true;

    private void Start()
    {
        isSoundOn = !AudioListener.pause;
        soundButton();
        soundButton();
        mainPanel = transform.Find("MainPanel").gameObject;
        victoryPanel = transform.Find("FinishPanel").Find("VictoryPanel").gameObject;
        failPanel = transform.Find("FinishPanel").Find("FailPanel").gameObject;
        mainPanel.SetActive(true);
        
    }
    public void soundButton()
    {
        AudioListener.pause = !AudioListener.pause;
        UnityEngine.UI.Image btn = GameObject.Find("SoundButton").GetComponent<UnityEngine.UI.Image>();
        isSoundOn = !isSoundOn;
        if (isSoundOn)
        {
            btn.sprite = soundOn;
        }
        else
        {
            btn.sprite = soundOff;
        }
            
    }
    public void restartLevel()
    {
        //PlaySound(sounds[0]);
        var SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(SceneIndex);
    }
    public void nextLevel()
    {
        var SceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(SceneManager.sceneCountInBuildSettings - 1 == SceneIndex)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneIndex + 1);
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
}
