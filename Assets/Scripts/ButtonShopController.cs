using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonShopController : MonoBehaviour
{

    [SerializeField] public int[] ToiletSkinCost = new int[17];
    [SerializeField] private GameObject _ScrollAreaBody;
    [SerializeField] private GameObject _ScrollAreaHead;
    [SerializeField] private Sprite _ActiveButton;
    [SerializeField] private Sprite _InactiveButton;
    [SerializeField] private GameObject _HeadButton;
    [SerializeField] private GameObject _BodyButton;
    private GameObject[] _GOArrUTI;
    private GameObject[] _GOArrButtonUse;
    private GameObject[] _GOArrButtonCurrency;
    private GameObject[] _GOArrButtonVideo;
    private string _ActivePanel = "head";

    public GameObject Money;
    private MoneyScript _MoneyScript;
    public GameObject SkinController;
    private ShopController SCScript;


    public GameObject MainToiletSkin;
    private ShopController MTSScript;

    public GameObject MainHeadSkin;
    private ShopController MHSScript;

    public GameObject MainNeckSkin;
    private ShopController MNSScript;

    public GameObject GameToiletSkin;
    private ShopController GTSScript;

    public GameObject GameHeadSkin;
    private ShopController GHSScript;

    public GameObject GameNeckSkin;
    private ShopController GNSScript;
    private void Start()
    {

        //SCScript = SkinController.GetComponent<ShopController>();
        _MoneyScript = Money.GetComponent<MoneyScript>();
        MTSScript = MainToiletSkin.GetComponent<ShopController>();
        MHSScript = MainHeadSkin.GetComponent<ShopController>();
        MNSScript = MainNeckSkin.GetComponent<ShopController>();
        GTSScript = GameToiletSkin.GetComponent<ShopController>();
        GHSScript = GameHeadSkin.GetComponent<ShopController>();
        GNSScript = GameNeckSkin.GetComponent<ShopController>();



        refresh();
        ClickBodyButton();
        ClickHeadButton();

        Debug.Log(this.GetType().Name + " is started " + this.name);
    }
    void refresh()
    {
        _GOArrUTI = GameObject.FindGameObjectsWithTag("UnlockedToiletImage");
        _GOArrButtonUse = GameObject.FindGameObjectsWithTag("ButtonUSE");
        _GOArrButtonCurrency = GameObject.FindGameObjectsWithTag("ButtonWithCurrency");
        _GOArrButtonVideo = GameObject.FindGameObjectsWithTag("ButtonWithVideo");
        /*if (_ActivePanel == "body")
        {
            _GOArrUTI = _GOArrUTI.Reverse().ToArray();
            _GOArrButtonUse = _GOArrButtonUse.Reverse().ToArray();
            _GOArrButtonCurrency = _GOArrButtonCurrency.Reverse().ToArray();
            _GOArrButtonVideo = _GOArrButtonVideo.Reverse().ToArray();
        }*/
        

        Debug.Log("LETGTH UTI " + _GOArrUTI.Length);
        Debug.Log("LETGTH _GOArrButtonUse " + _GOArrButtonUse.Length);
        Debug.Log("LETGTH _GOArrButtonCurrency " + _GOArrButtonCurrency.Length);
        Debug.Log("LETGTH  _GOArrButtonVideo " + _GOArrButtonVideo.Length);
        Debug.Log("LETGTH PROGRESS " + SaveManager.CurrentState.OpenHeadSkin.Length);
        for(int i = 0; i < SaveManager.CurrentState.OpenHeadSkin.Length; i++)
        {
            Debug.Log(i + " " + SaveManager.CurrentState.OpenHeadSkin[i]);
        }
        TextMeshPro TMP;
        for (int i = 0; i < _GOArrUTI.Length; i++)
        {
            if(_ActivePanel == "body")
            {
                if (SaveManager.CurrentState.OpenToiletSkin[i])
                {
                    _GOArrUTI[i].SetActive(true);
                    _GOArrButtonUse[i].SetActive(true);
                    _GOArrButtonCurrency[i].SetActive(false);
                    
                    _GOArrButtonVideo[i].SetActive(false);
                }
                else
                {
                    _GOArrUTI[i].SetActive(true);
                    _GOArrButtonUse[i].SetActive(false);
                    _GOArrButtonCurrency[i].SetActive(true);
                    _GOArrButtonVideo[i].SetActive(false);
                    /*TMP = _GOArrButtonCurrency[i].transform.Find("Hor/Text (TMP)").GetComponent<TextMeshPro>();
                    TMP.text = SaveManager.Instance.SavedData.ToiletSkinCost[i].ToString();*/
                }
            }
            else if(_ActivePanel == "head")
            {
                if (SaveManager.CurrentState.OpenHeadSkin.Length > i && SaveManager.CurrentState.OpenHeadSkin[i])
                {
                    _GOArrUTI[i].SetActive(true);
                    _GOArrButtonUse[i].SetActive(true);
                    _GOArrButtonCurrency[i].SetActive(false);
                    _GOArrButtonVideo[i].SetActive(false);
                }
                else
                {
                    _GOArrUTI[i].SetActive(true);
                    _GOArrButtonUse[i].SetActive(false);
                    _GOArrButtonCurrency[i].SetActive(false);
                    _GOArrButtonVideo[i].SetActive(true);
                }
            }
            
        }
    }
    void ChangePanel()
    {
        _MoneyScript.UpdateInfo();
        for (int i = 0; i < _GOArrUTI.Length; i++)
        {
            _GOArrUTI[i].SetActive(true);
            _GOArrButtonUse[i].SetActive(true);
            _GOArrButtonCurrency[i].SetActive(true);
            _GOArrButtonVideo[i].SetActive(true);
        }
        switch (_ActivePanel)
        {
            case "body":
                _HeadButton.GetComponent<Image>().sprite = _InactiveButton;
                _BodyButton.GetComponent<Image>().sprite = _ActiveButton;
                _ScrollAreaHead.SetActive(false);
                _ScrollAreaBody.SetActive(true);
                break;
            case "head":
                _HeadButton.GetComponent<Image>().sprite = _ActiveButton;
                _BodyButton.GetComponent<Image>().sprite = _InactiveButton;
                _ScrollAreaHead.SetActive(true);
                _ScrollAreaBody.SetActive(false);
                break;
        }
        refresh();
    }
    public void ClickHeadButton()
    {
        Debug.Log("CLICK HEAD BUTTON");
        _ActivePanel = "body";
        ChangePanel();
        _ActivePanel = "head";
        ChangePanel();
    }
    public void ClickBodyButton()
    {
        Debug.Log("CLICK BODY BUTTON");
        _ActivePanel = "head";
        ChangePanel();
        _ActivePanel = "body";
        ChangePanel();
    }
    public void clickUseButton()
    {
        Debug.Log(SaveManager.CurrentState.currentHeadIndex);
        int index = 0;
        for(int i = 0; i < _GOArrButtonUse.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.GetInstanceID() == _GOArrButtonUse[i].GetInstanceID())
            {
               index = i;
            }
        }
        Debug.Log("INDEX " + index); 
        if(_ActivePanel == "body")

        {
            
            SaveManager.CurrentState.currentToiletIndex = index;
            MTSScript.ChangeSkin("Toilet", SaveManager.CurrentState.currentHeadIndex, index);
            GTSScript.ChangeSkin("Toilet", SaveManager.CurrentState.currentHeadIndex, index);
        }
        else

        {
            SaveManager.CurrentState.currentHeadIndex = index;
            MHSScript.ChangeSkin("Head", index, SaveManager.CurrentState.currentToiletIndex);
            MNSScript.ChangeSkin("NeckSprite", index, SaveManager.CurrentState.currentToiletIndex);
            GHSScript.ChangeSkin("Head", index, SaveManager.CurrentState.currentToiletIndex);
            GNSScript.ChangeSkin("NeckMaterial", index, SaveManager.CurrentState.currentToiletIndex);
        }
        SaveManager.SaveState();

    }
    public void getVideoSkin()
    {
        int index = 0;
        for (int i = 0; i < _GOArrButtonVideo.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.GetInstanceID() == _GOArrButtonVideo[i].GetInstanceID())
            {
                index = i;
            }
        }
        if (_ActivePanel == "body")
        {
            SaveManager.CurrentState.OpenToiletSkin[index] = true;

        }
        else
        {
            SaveManager.CurrentState.OpenHeadSkin[index] = true;
        }
        if (index == SaveManager.CurrentState.NewSkinNumber)
        {
            SaveManager.CurrentState.NewSkinPercent = 0;
        }
        _ActivePanel = "body";
        ChangePanel();
        _ActivePanel = "head";
        ChangePanel();
    }
    public void clickVideoButton()
    {
        
#if UNITY_EDITOR
        getVideoSkin();
#else
        YaSDK.ShowRewardedVideo(onClose: () =>
        {
            if (YaSDK._isRewarded)
                getVideoSkin();
        });
#endif
        SaveManager.SaveState();

    }
    public void clickCurrencyButton()
    {
        int index = 0;
        for (int i = 0; i < _GOArrButtonCurrency.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.GetInstanceID() == _GOArrButtonCurrency[i].GetInstanceID())
            {
                index = i;
            }
        }
        Debug.Log("COST: " + ToiletSkinCost[index]);
        if (SaveManager.CurrentState.Coins < ToiletSkinCost[index]) return;
        SaveManager.CurrentState.Coins -= ToiletSkinCost[index];
        if (_ActivePanel == "body")
        {
            SaveManager.CurrentState.OpenToiletSkin[index] = true;
        }
        else
        {
            SaveManager.CurrentState.OpenHeadSkin[index] = true;
        }
       
        _ActivePanel = "head";
        ChangePanel();
        _ActivePanel = "body";
        ChangePanel();
        SaveManager.SaveState();
    }
}