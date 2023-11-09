using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroProgressScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Image _HeadImageBlack;
    private Image _HeadImage;
    private TextMeshProUGUI _PercentText;
    private int _skinNumber = 0;
    private float _skinPercent = 0;
    private bool anim = false;
    public Sprite[] SpriteArray = new Sprite[11];


    void Start()
    {
        //SaveManager.CurrentState.NewSkinNumber = 8;
        // Debug.Log("NewSkinNumber " + SaveManager.Instance.SavedData.NewSkinNumber);
        checkNewSkin();
        _skinPercent = SaveManager.CurrentState.NewSkinPercent;
        _skinNumber = SaveManager.CurrentState.NewSkinNumber;
        _HeadImageBlack = transform.Find("HeadImageBlack").GetComponent<Image>();
        _HeadImage = transform.Find("HeadImage").GetComponent<Image>();

        _PercentText = transform.Find("PercentText").GetComponent<TextMeshProUGUI>();

        
        if (_skinNumber == 6 || _skinNumber == 10 || _skinNumber == 11)
        {
            _HeadImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
            _HeadImageBlack.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        _HeadImage.sprite = SpriteArray[_skinNumber];
        _HeadImageBlack.sprite = SpriteArray[_skinNumber];
        Debug.Log("skinPercent: " + _skinPercent);
        win();
        Debug.Log(this.GetType().Name + " is started " + this.name);
        Debug.Log("HERO PROGRESS ____________________________________");
        Debug.Log(_skinNumber);
        Debug.Log(SaveManager.CurrentState.NewSkinNumber);

    }

    private void checkNewSkin()
    {
        Debug.Log("SaveManager.CurrentState.NewSkinNumber " + SaveManager.CurrentState.NewSkinNumber);
        //if (SaveManager.CurrentState.NewSkinNumber == 3) SaveManager.CurrentState.NewSkinNumber += 3;
        if (SaveManager.CurrentState.OpenHeadSkin[SaveManager.CurrentState.NewSkinNumber])
        {
            SaveManager.CurrentState.NewSkinNumber++;
            checkNewSkin();
        }
        else
        {
            return;
        }
    }
    private void win()
    {
        SaveManager.CurrentState.NewSkinPercent = (int)_skinPercent + 20;
        _skinPercent += 2;

        anim = true;
        SaveManager.SaveState();
    }
    // Update is called once per
    // 
    void Update()
    {
        if (!anim) return;

        
        if (SaveManager.CurrentState.NewSkinPercent == 100)
        {
            
            SaveManager.CurrentState.NewSkinPercent = 0;
            SaveManager.CurrentState.NewSkinNumber++;
            //giveSkin();
        }
        


        if (Math.Round(_skinPercent) % 20 != 0 )
        {
            _skinPercent += 0.04f;
            _PercentText.text = Math.Round(_skinPercent).ToString() + "%";
            _HeadImage.fillAmount = _skinPercent/100;
        }
        else
        {
            anim = false;
        }

    }
}
