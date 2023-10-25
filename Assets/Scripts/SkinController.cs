using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public Sprite[] ToiletSprites;
    public Sprite[] NeckSprites;
    public Material[] NeckMaterials;
    public string WhatChange;


    private SkeletonAnimation _ScibidiAnimation;
    private UnityEngine.LineRenderer _LineRenderer;

    public GameObject ButtonShopController;
    private ButtonShopController BSCScript;


// Start is called before the first frame update
void Start() 
        { 
        BSCScript = ButtonShopController.GetComponent<ButtonShopController>();

        Progress.Instance.PlayerInfo.OpenHeadSkin[0] = true;
        Progress.Instance.PlayerInfo.OpenHeadSkin[2] = true;
        Progress.Instance.PlayerInfo.OpenToiletSkin[0] = true;
        ChangeSkin(WhatChange, Progress.Instance.PlayerInfo.currentHeadIndex, Progress.Instance.PlayerInfo.currentToiletIndex);
       
        
    }
    public void ChangeSkin(string subjectName, int currentHeadIndex, int currentToiletIndex)
    {
        switch (subjectName)
        {
            case "Head":
                _ScibidiAnimation = GetComponent<Player>().skeletonAnimation;
                if (currentHeadIndex > 5) currentHeadIndex++;
                _ScibidiAnimation.initialSkinName = "skin " + (currentHeadIndex + 1);
                _ScibidiAnimation.Initialize(true);
                break;
            case "Toilet":
                Debug.Log("I try to change TOILET SKIN");
                Debug.Log(GetComponent<Image>());
                Debug.Log(GetComponent<SpriteRenderer>());
                string text = string.Empty;
                foreach (var component in GetComponents(typeof(Component)))
                {
                    text += component.GetType().ToString() + " ";
                }
                if (GetComponent<Image>() == null)
                {
                    GetComponent<SpriteRenderer>().sprite = ToiletSprites[currentToiletIndex + 1];
                    if (currentToiletIndex == 5)
                    {
                        GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                    else
                    {
                        GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 270f);
                    }
                    break;
                }
                //Debug.Log(currentToiletIndex);
                GetComponent<Image>().overrideSprite = ToiletSprites[currentToiletIndex + 1];
                //Debug.Log(GetComponent<Image>());
                //Debug.Log(currentToiletIndex);
                if (currentToiletIndex == 5)
                {
                    GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else
                {
                    GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 270f);
                }

                break;
            case "NeckSprite":
                if (currentHeadIndex > 5) currentHeadIndex++;
                GetComponent<Image>().sprite = NeckSprites[currentHeadIndex];
                break;
            case "NeckMaterial":
                if (currentHeadIndex > 5) currentHeadIndex++;
                _LineRenderer = GetComponent<LineRenderer>();
                _LineRenderer.material = NeckMaterials[currentHeadIndex];
                break;
        }
    }
    void buyToiletSkin()
    {

    }
    
}

