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
        if(typeOfCamera == "Jetpuck")
            {
                if (!die)
            {
               enemy_Rigidbody.isKinematic = true;
            }
            else
            {
                enemy_Rigidbody.isKinematic = false;
            }
        
        }
        if (Vector3.Distance(this.transform.position, ScibidiHead.transform.position) < 2.3f && !die)
        {
            if(!(skeletonAnimation.AnimationName == "scare"))
            {
                //PlaySound(sounds[0]);
                skeletonAnimation.AnimationName = "scare";
            }
        }
        else if(skeletonAnimation.AnimationName == "dieloop" || skeletonAnimation.AnimationName == "die_loop")
        {
            
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
        GetComponent<PolygonCollider2D>().enabled = false;

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision detected");
            Explosion.SetActive(true);
            gameObject.tag = "EnemyDead";
        Debug.Log(typeOfCamera + " Jetpuck");
            if(typeOfCamera == "Jetpuck")
        {
            skeletonAnimation.AnimationName = "die_loop";
        }
        else
        {
            skeletonAnimation.AnimationName = "dieloop";
        }
            Invoke("bodyHide", 0.3f);
            transform.Find("EnemyCameraHead").gameObject.SetActive(true);
            die = true;
            
            an.enabled = true;
        
        

    }
    public void playerLose()
    {
        isPlayerLose = true;
    }
}
