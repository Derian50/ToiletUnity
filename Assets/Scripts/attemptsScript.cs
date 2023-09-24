using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class attemptsScript : MonoBehaviour
{
    [SerializeField] public bool isEnabled = false;
    [SerializeField] public int countAttempts = 1;
    private TextMeshProUGUI _TextMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(isEnabled);

        _TextMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _TextMeshPro.SetText("X " + countAttempts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void newAttempts()
    {
        if(isEnabled)
        {
            countAttempts--;
            _TextMeshPro.SetText("X " + countAttempts.ToString());
            if(countAttempts == 0)
            {
                _TextMeshPro.color = Color.red;
            }
        }
    }
}
