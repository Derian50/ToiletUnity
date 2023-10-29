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
        Debug.Log("NewSkinNumber " + SaveManager.Instance.SavedData.NewSkinNumber);
        checkNewSkin();
        _skinPercent = SaveManager.Instance.SavedData.NewSkinPercent;
        _skinNumber = SaveManager.Instance.SavedData.NewSkinNumber;
        _HeadImageBlack = transform.Find("HeadImageBlack").GetComponent<Image>();
        _HeadImage = transform.Find("HeadImage").GetComponent<Image>();
        _PercentText = transform.Find("PercentText").GetComponent<TextMeshProUGUI>();

        _HeadImage.sprite = SpriteArray[_skinNumber];
        _HeadImageBlack.sprite = SpriteArray[_skinNumber];

        Debug.Log("skinPercent: " + _skinPercent);
        win();
        Debug.Log(this.GetType().Name + " is started " + this.name);
    }

    private void checkNewSkin()
    {
        if (SaveManager.Instance.SavedData.OpenHeadSkin[SaveManager.Instance.SavedData.NewSkinNumber])
        {
            SaveManager.Instance.SavedData.NewSkinNumber++;
            checkNewSkin();
        }
        else
        {
            return;
        }
    }
    private void win()
    {
        SaveManager.Instance.SavedData.NewSkinPercent = (int)_skinPercent + 20;
        _skinPercent += 2;
        anim = true;
    }
    // Update is called once per
    // 
    void Update()
    {
        if (!anim) return;

        
        if (SaveManager.Instance.SavedData.NewSkinPercent == 100)
        {
            
            SaveManager.Instance.SavedData.NewSkinPercent = 0;
            
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
