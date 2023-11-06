using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Toilet")
        {
            GameObject.Find("ScibidiHeadPivot").GetComponent<Player>().TNTLose();
        }
        Debug.Log("RAY COLLLISION WITH " + collision.gameObject.tag);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player") return;
        Debug.Log("RAY DON'T COLLLISION WITH " + collision.gameObject.tag);
        Debug.Log(gameObject.transform.localScale);
        gameObject.transform.Find("Sparks").gameObject.SetActive(false);
        gameObject.transform.localScale = new Vector3(70, 1, 1);
    }
}
