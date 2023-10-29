using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyRewardScript : MonoBehaviour
{
    private int _moneyRewardCount;
    private TextMeshProUGUI _moneyRewardText;
    [SerializeField] GameObject OW;
    private OscillatingWheel _OWScript;
    void Start()
    {
        _moneyRewardCount = 200;
        _OWScript = OW.GetComponent<OscillatingWheel>();
        _moneyRewardText = transform.Find("MoneyRewardText").GetComponent<TextMeshProUGUI>();
        Debug.Log("multiplier " + _OWScript.mult);
        _moneyRewardText.text = (_moneyRewardCount * _OWScript.mult).ToString();
        Debug.Log(this.GetType().Name + " is started " + this.name);
    }
    private void newReward(int multiplier)
    {
        Debug.Log("multiplier " + multiplier);
        _moneyRewardText.text = (_moneyRewardCount * multiplier).ToString();
    }
    private void Update()
    {
        _moneyRewardText.text = (_moneyRewardCount * _OWScript.mult).ToString();
    }
}
