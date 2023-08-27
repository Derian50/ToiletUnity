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



    private void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }
    void Start()
    {
        lrScript = lr.GetComponent<LineRenderScript>();
        rb = GetComponent<Rigidbody2D>();  
    }
    void FixedUpdate(){
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

        Debug.Log("Someone collision");
        if (other.gameObject.name == "enemyCamera")
        {
            inNeckReverse = true;
            lrScript.reverseNeck(lose);
        }
        if (other.gameObject.tag == "spikes")
        {
            Debug.Log("Someone died");
            inNeckReverse = true;
            lose = true;
            lrScript.reverseNeck(lose);

            skeletonAnimation.AnimationName = "touchspikeloop";
        }

    }
}
