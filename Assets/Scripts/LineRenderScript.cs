using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Spine.Unity;

[RequireComponent(typeof(LineRenderer))]

public class LineRenderScript : MonoBehaviour
{
    //childObject.transform.parent.gameObject
    UnityEngine.LineRenderer lr;
    [SerializeField] Material mat;
    [SerializeField] int minDistance = 2;
    [SerializeField] GameObject ScibidiHead;
    private bool lose = false;
    private bool pause = false;
    private float shX;
    private float shY;
    private float counter = 0;
    private bool isNeckReverse = false;
    List<Vector3> positions = new List<Vector3>();

    List<float> rotatesZ = new List<float>();

    void Start()
    {

        shX = ScibidiHead.transform.position.x;
        shY = ScibidiHead.transform.position.y;
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 3;
        lr.material = mat;
        lr.numCornerVertices = 1;
        positions.Add(new Vector3(shX, shY - 0.4f, 0));
        positions.Add(new Vector3(shX, shY, 0));
        positions.Add(new Vector3(shX, shY, 0));
        rotatesZ.Add(360);
        rotatesZ.Add(360);
        rotatesZ.Add(360);
        lr.SetPositions(positions.ToArray());
    }
    private void restartLevel()
    {
        var SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(SceneIndex);
    }
    private void nextLevel()
    {
        var SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(SceneIndex + 1);
    }
    private void Losing()
    {
        ScibidiHead.SetActive(false);
        ScibidiHead.GetComponent<Player>().skeletonAnimation.enabled = false;
        Invoke("restartLevel", 1);

    }
    private void FixedUpdate()
    {
        if (pause) return;
        //«десь что-то и SceneManager забыл, во странный, надо бы его позже отсюда убрать
        //ƒа и голове именно тут двигаетс€ назад, нехорошо это как-то...
        if (isNeckReverse)
        {
            if (positions.Count > 2 && rotatesZ.Count > 2)
            {
                ScibidiHead.transform.transform.eulerAngles = new Vector3(0, 0, rotatesZ[rotatesZ.Count - 1]);
                rotatesZ.RemoveRange(rotatesZ.Count - 1, 1);

                ScibidiHead.transform.position = positions[positions.Count - 1];
                positions.RemoveAt(positions.Count - 1);
                lr.positionCount--;
                lr.SetPositions(positions.ToArray());
            }
            else if (lose)
            {
                pause = true;
                isNeckReverse = false;
                ScibidiHead.GetComponent<Player>().skeletonAnimation.AnimationName = "lose";
                lr.enabled = false;
                Invoke("Losing", 1);
            }
            else
            {
                Invoke("nextLevel", 1);
            }
            
        }
        else
        {
            
            shX = ScibidiHead.transform.position.x;
            shY = ScibidiHead.transform.position.y;
            counter++;
            if (counter < minDistance)
            {
                // Debug.Log("return");
                return;
            }
            counter = 0;
            /* if ((Math.Abs(positions.Last().x) - Math.Abs(shX)) < minDistance && (Math.Abs(shX)) - (Math.Abs(positions.Last().x)) > minDistance)
             {
                 return;
             }
             if ((Math.Abs(positions.Last().y) - Math.Abs(shY)) < minDistance && (Math.Abs(shY)) - (Math.Abs(positions.Last().y)) > minDistance)
             {
                 return;
             }*/
            rotatesZ.Add(ScibidiHead.transform.rotation.eulerAngles.z);

            positions.Add(new Vector3(shX, shY, 0));
            lr.positionCount++;
            lr.SetPositions(positions.ToArray());
        }
        
    }
  
    public void reverseNeck(bool lose)
    {
        this.lose = lose;
        isNeckReverse = true;
    }

}
