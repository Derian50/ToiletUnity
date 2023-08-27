using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Spine;
//using System.Diagnostics;

public class EnemyCamera : MonoBehaviour
    {
        private SkeletonAnimation skeletonAnimation;
        private Transform go;
        private Animator an;
    private bool die = false;
    private GameObject ScibidiHead;
    private AnimatorClipInfo[] clipInfo;

    void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        go = transform.Find("Boom");
        
    }
    void Start()
    {
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
            skeletonAnimation.AnimationName = "scare";
        }
        else if(skeletonAnimation.AnimationName == "dieloop")
        {
            
        }
        else
        {
            skeletonAnimation.AnimationName = "idle";
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision detected");
        die = true;
        skeletonAnimation.AnimationName = "dieloop";
        an.enabled = true;

    }
}
