using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovementJoystick : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;
    Vector2 tmp = new Vector2();
    //0, -3.5
    void Start()
    {
        
        joystickOriginalPos = joystickBG.transform.position;
        joystickRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }

    public void Update(){
       // Debug.Log(Input.mousePosition);
        //Debug.Log(joystick.transform.position);
        //Debug.Log("--");
    }
    public void PointerDown(){
        
        tmp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        joystick.transform.position = tmp;
        joystickBG.transform.position = tmp;
        
        joystickTouchPos = tmp;
        
    }

    public void Drag(BaseEventData baseEventData){
        tmp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = tmp;
        joystickVec = (dragPos - joystickTouchPos).normalized;

        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

        if(joystickDist < joystickRadius){
            joystick.transform.position = joystickTouchPos + joystickVec * joystickDist;
        }else{
            joystick.transform.position = joystickTouchPos + joystickVec * joystickRadius;
        }
    }

    public void PointerUp(){
       // Debug.Log(3);
       // joystickVec = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }
    public void Click(){
        Debug.Log("Click");
    }

}
