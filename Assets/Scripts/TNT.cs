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
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Toilet" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "enemyRocket" || other.gameObject.tag == "EnemyHead" || other.gameObject.tag == "Explosion" || other.gameObject.tag == "EnemyDead")
        {
            
            if (Vector3.Distance(this.transform.position, GameObject.Find("toilet").transform.position) < 2.4f)
            {
                Debug.Log("PLAYER MUST DIE");
                GameObject.Find("ScibidiHeadPivot").GetComponent<Player>().TNTLose();
            }
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (Vector3.Distance(this.transform.position, enemy.transform.position) < 2.4f)
                    {
                        Debug.Log("PLAYER MUST DIE");
                        enemy.GetComponent<EnemyCamera>().TNTLose();
                    }
                }
           
            {

            }
            transform.Find("coal").gameObject.SetActive(true);
            transform.Find("TNTSprite").gameObject.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            ExplosionSmoke.SetActive(true);
            boom = true;
            boomVector3 = transform.position;

            Debug.Log("TNT Collision detected");
        }
            


    }
}
