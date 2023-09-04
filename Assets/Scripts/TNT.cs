using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    private GameObject ExplosionSmoke;
    private Vector3 boomVector3 = Vector3.zero;
    private bool boom = false;
    private void Start()
    {
        ExplosionSmoke = transform.Find("ExplosionSmoke").gameObject;
        ExplosionSmoke.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (boomVector3 != Vector3.zero)
        {
            ExplosionSmoke.transform.position = boomVector3;
            ExplosionSmoke.transform.rotation = Quaternion.identity;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (boom) return;
        //if (other.gameObject.tag == "Rope") return;
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyHead")
        {
            boom = true;
            boomVector3 = transform.position;

            Debug.Log("TNT Collision detected");
            transform.Find("TNTSprite").gameObject.SetActive(false);
            ExplosionSmoke.SetActive(true);
        }
            


    }
}
