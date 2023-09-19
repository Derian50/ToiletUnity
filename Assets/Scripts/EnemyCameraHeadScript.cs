using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCameraHeadScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.tag);
        if(collision.gameObject.tag == "Ray" || collision.gameObject.tag == "border")
        {
            gameObject.SetActive(false);
        }
    }
}
