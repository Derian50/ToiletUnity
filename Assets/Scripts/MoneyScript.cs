using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int _moneyCount;
    private TextMeshProUGUI _moneyText;
    void Start()
    {
        _moneyCount = Progress.Instance.PlayerInfo.Coins;
        _moneyText = transform.Find("MoneyText").GetComponent<TextMeshProUGUI>();
        _moneyText.text = _moneyCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
