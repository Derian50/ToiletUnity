using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public MovementJoystick movementJoystick;
    public float playerSpeed;
    private Rigidbody2D rb;
    public Canvas canvas;
    public GameObject lr;
    private bool lose = false;
    private LineRenderScript lrScript;
    public bool inNeckReverse = false;
    //private GameObject Explosion;
    private GameObject Firework;
    private Vector3 boomVector3 = Vector3.zero;


    private void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }
    void Start()
    {
        //Explosion = transform.Find("Explosion").gameObject;
        //Explosion.SetActive(false);
        Firework = transform.Find("Firework").gameObject;
        Firework.SetActive(false);
        lrScript = lr.GetComponent<LineRenderScript>();
        rb = GetComponent<Rigidbody2D>();  
    }
    void FixedUpdate(){
        
        
        if (boomVector3 != Vector3.zero)
        {
            //Explosion.transform.position = boomVector3;
            Firework.transform.position = boomVector3;
            Firework.transform.rotation = Quaternion.identity;
        }
        if (inNeckReverse)
        {
            rb.velocity = Vector2.zero;

        }
        else
        {

            
            if (movementJoystick.joystickVec.y != 0 || movementJoystick.joystickVec.x != 0)
            {
                Vector2 moveVector = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed);
                // transform.up = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed); 
                transform.up = Vector2.MoveTowards(transform.up, moveVector, Time.deltaTime * 10); //10 is velocity rotation
                rb.velocity = moveVector.normalized * playerSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        
    }
    void OnCollisionEnter2D(Collision2D other)
    {

        Debug.Log("Someone collision " + other.gameObject.tag);
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyHead" || other.gameObject.tag == "EnemyDead")
        {
            //boomVector3 = transform.position;
           // Explosion.SetActive(true);
            inNeckReverse = true;
            lrScript.startReverseNeck("nothing");
            //lrScript.startReverseNeck("win");
        }
        if (other.gameObject.tag == "spikes")
        {
            Debug.Log("Someone died");
            inNeckReverse = true;
            lose = true;
            lrScript.startReverseNeck("lose");

            skeletonAnimation.AnimationName = "touchspikeloop";
        }
        if (other.gameObject.tag == "Rope")
        {

            if(GameObject.Find("Rope") != null)
            {
                boomVector3 = transform.position;
                Firework.SetActive(true);
                inNeckReverse = true;
                lrScript.startReverseNeck("win");
                GameObject.Find("Rope").SetActive(false);
            }
            
        }
        if (other.gameObject.tag == "TNT")
        {
            boomVector3 = transform.position;
            inNeckReverse = true;
            lose = true;
            lrScript.startReverseNeck("lose");
        }

    }
}
