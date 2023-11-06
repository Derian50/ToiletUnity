using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public MovementJoystick movementJoystick;
    public float playerSpeed;
    private Rigidbody2D _rb;
    public Canvas canvas;
    public GameObject lr;
    private bool _lose = false;
    private LineRenderScript _lrScript;
    public bool inNeckReverse = false;
    //private GameObject Explosion;
    private GameObject Firework;
    private Vector3 boomVector3 = Vector3.zero;
    public Vector2 lastHeadVelocity = Vector2.zero;
    private bool _isMove = false;

    

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
        _lrScript = lr.GetComponent<LineRenderScript>();
        _rb = GetComponent<Rigidbody2D>();  
        Debug.Log(this.GetType().Name + " is started " + this.name);
    }
    void FixedUpdate(){

       // if (playerSpeed == 0) return;
       if(_rb.velocity.x != 0 && _rb.velocity.y != 0) lastHeadVelocity = _rb.velocity;
        if (boomVector3 != Vector3.zero)
        {
            //Explosion.transform.position = boomVector3;
            Firework.transform.position = boomVector3;
            Firework.transform.rotation = Quaternion.identity;
        }
        if (inNeckReverse)
        {
            _rb.velocity = Vector2.zero;

        }
        else
        {

            
            if (movementJoystick.joystickVec.y != 0 || movementJoystick.joystickVec.x != 0)
            {
                
                Vector2 moveVector = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed);
                // transform.up = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed); 
                transform.up = Vector2.MoveTowards(transform.up, moveVector, Time.deltaTime * 10); //10 is velocity rotation
                _rb.velocity = moveVector.normalized * playerSpeed;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }
        }
        
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision player with: " + other.gameObject.tag);
        if(other.gameObject.tag == "Rocket")
        {
            _lrScript._isRocket = true;
        }
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyHead" || other.gameObject.tag == "EnemyDead") //|| other.gameObject.tag == "EnemyHead" || other.gameObject.tag == "EnemyDead")
        {
            //boomVector3 = transform.position;
            // Explosion.SetActive(true);
            _lrScript.startReverseNeck("nothing"); 
            inNeckReverse = true;

            //_lrScript.startReverseNeck("win");
        }
        if (other.gameObject.tag == "spikes" || other.gameObject.tag == "Ray" || (other.gameObject.tag == "enemyRocket" && !_lrScript._isRocket))
        {
            inNeckReverse = true;
            _lose = true;
            _lrScript.startReverseNeck("lose");

            skeletonAnimation.AnimationName = "touchspikeloop";
        }
        if (other.gameObject.tag == "Rope")
        {

            if(GameObject.Find("Rope") != null)
            {
                boomVector3 = transform.position;
                Firework.SetActive(true);
                _lrScript.startReverseNeck("nothing");
                GameObject.Find("Rope").SetActive(false);
            }
            
        }
        if(other.gameObject.tag == "Ice")
        {
            _lrScript.startReverseNeck("nothing");
        }
        if (other.gameObject.tag == "TNT")
        {
            TNTLose();
        }

    }
    public void TNTLose()
    {
        Debug.Log("LOSEEEEEEEEEEEEEEEEEEEEe");
        boomVector3 = transform.position;
        inNeckReverse = true;
        _lose = true;
        _lrScript.startReverseNeck("lose");
    }

}
