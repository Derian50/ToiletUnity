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
        private SkeletonAnimation skeletonAnimation;
        private Transform go;
        private Animator an;
        private bool die = false;
        private GameObject ScibidiHead;
        private AnimatorClipInfo[] clipInfo;
        private bool isPlayerLose = false;

        private GameObject Explosion;

    void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        go = transform.Find("Boom");
        
    }
    void Start()
    {
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
        if (Vector3.Distance(this.transform.position, ScibidiHead.transform.position) < 2.3f && !die)
        {
            if(!(skeletonAnimation.AnimationName == "scare"))
            {
                PlaySound(sounds[0]);
                skeletonAnimation.AnimationName = "scare";
            }
        }
        else if(skeletonAnimation.AnimationName == "dieloop")
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
            Invoke("bodyHide", 2f);
            transform.Find("EnemyCameraHead").gameObject.SetActive(true);
            die = true;
            skeletonAnimation.AnimationName = "dieloop";
            an.enabled = true;
        
        

    }
    public void playerLose()
    {
        isPlayerLose = true;
    }
}
