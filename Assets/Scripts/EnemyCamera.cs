using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Spine;
//using System.Diagnostics;

public class EnemyCamera : Sounds
    {
        [SerializeField] string typeOfCamera;
        private SkeletonAnimation skeletonAnimation;
        private Transform go;
        private Animator an;
        private bool die = false;
        private GameObject ScibidiHead;
        private AnimatorClipInfo[] clipInfo;
        private bool isPlayerLose = false;
    Rigidbody2D enemy_Rigidbody;
    private GameObject Explosion;

    void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        go = transform.Find("Boom");
        
    }
    void Start()
    {
        if(typeOfCamera == "Jetpuck")
        {
            Debug.Log("JUTPUCK");
            enemy_Rigidbody = GetComponent<Rigidbody2D>();
        }
        Explosion = transform.Find("Explosion").gameObject;
        Explosion.SetActive(false);
        ScibidiHead = GameObject.Find("ScibidiHeadPivot");
       skeletonAnimation.AnimationName = "idle";
       an = go.GetComponent<Animator>();
       Debug.Log(go.name);
        
        an.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // Debug.Log(ScibidiHead.GetComponent<Rigidbody2D>().velocity);
        if (typeOfCamera == "Jetpuck")
            {
            if (!die)
            {
               enemy_Rigidbody.isKinematic = true;
            }
            else
            {
                enemy_Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
        
        }
        if (skeletonAnimation.AnimationName == "dieloop" || skeletonAnimation.AnimationName == "die_loop")
        {

        }
        else if (Vector3.Distance(this.transform.position, ScibidiHead.transform.position) < 2.3f && !die)
        {
            if(!(skeletonAnimation.AnimationName == "scare"))
            {
                //PlaySound(sounds[0]);
                skeletonAnimation.AnimationName = "scare";
            }
        }
        else if (isPlayerLose)
        {
            skeletonAnimation.AnimationName = "win";
        }
        else if (!isPlayerLose)
        {
            skeletonAnimation.AnimationName = "idle";
        }

    }
    private void bodyHide()
    {
        GetComponent<BoxCollider2D>().enabled = false;

    }
    private void DestroyBody()
    {
        gameObject.SetActive(false);
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "border")
        {
            Invoke("DestroyBody", 1);
        }
        Debug.Log("ENEMY COLLISION " + other.gameObject.tag);
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Untagged" || other.gameObject.tag == "Platform") return;
        Debug.Log("VELOCITY " + ScibidiHead.GetComponent<Player>().lastHeadVelocity);
        
        Explosion.SetActive(true);
        gameObject.tag = "EnemyDead";
        if(typeOfCamera == "Jetpuck")
        {
            skeletonAnimation.AnimationName = "die_loop";
        }
        else
        {
            skeletonAnimation.AnimationName = "dieloop";
        }
            transform.Find("EnemyCameraHead").gameObject.SetActive(true);
            transform.Find("EnemyCameraHead").GetComponent<Rigidbody2D>().velocity = ScibidiHead.GetComponent<Player>().lastHeadVelocity*2;
            GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = false;
            die = true;
            
            an.enabled = true;
        
        

    }
    public void playerLose()
    {
        isPlayerLose = true;
    }
    public void TNTLose()
    {
        if (typeOfCamera == "Jetpuck")
        {
            skeletonAnimation.AnimationName = "die_loop";
        }
        else
        {
            skeletonAnimation.AnimationName = "dieloop";
        }
        Explosion.SetActive(true);
        gameObject.tag = "EnemyDead";
        transform.Find("EnemyCameraHead").gameObject.SetActive(true);
        transform.Find("EnemyCameraHead").GetComponent<Rigidbody2D>().velocity = ScibidiHead.GetComponent<Player>().lastHeadVelocity * 2;
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
        die = true;

        an.enabled = true;
    }
}
