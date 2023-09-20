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

public class LineRenderScript : Sounds
{
    //childObject.transform.parent.gameObject
    UnityEngine.LineRenderer lr;
    GameObject Confetti;
    [SerializeField] GameObject UI;
    UIScript uiScript;
    [SerializeField] Material mat;
    [SerializeField] int minDistance = 2;
    [SerializeField] GameObject ScibidiHead;
    public bool lose = false;
    public bool win = false;
    private bool pause = false;
    private SkeletonAnimation ScibidiAnimation;
    private float shX;
    private float shY;
    private float _startShX;
    private float _startShY;
    private float counter = 0;
    private bool isNeckReverse = false;
    List<Vector3> positions = new List<Vector3>();
    private GameObject Sparks;

    List<float> rotatesZ = new List<float>();

    void Start()
    {
        PlaySound(sounds[0]);
        uiScript = UI.GetComponent<UIScript>();
        ScibidiAnimation = ScibidiHead.GetComponent<Player>().skeletonAnimation;
        shX = ScibidiHead.transform.position.x;
        shY = ScibidiHead.transform.position.y;
        _startShX = shX;
        _startShY = shY;    
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 3;
        lr.material = mat;
        lr.numCornerVertices = 1;
        positions.Add(new Vector3(shX, shY - 0.4f, 0));
        positions.Add(new Vector3(shX, shY - 0.2f, 0));
        positions.Add(new Vector3(shX, shY, 0));
        lr.SetPositions(positions.ToArray());
        rotatesZ.Add(360);
        rotatesZ.Add(360);
        rotatesZ.Add(360);

        Sparks = transform.Find("Sparks").gameObject;
        Sparks.SetActive(false);

        ScibidiAnimation.AnimationName = "idle";
        
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if ((sceneIndex + 1) % 4 == 0)
        {
            YaSDK.ShowFullscreenAdv();
        }

    }
    private void winUIScript()
    {
        uiScript.win();
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

    private void Dying()
    {
        PlaySound(sounds[2]);
        ScibidiHead.SetActive(false);
        ScibidiAnimation.enabled = false;
    }
    private void Losing()
    {
        PlaySound(sounds[1]);
        PlaySound(sounds[6]);
        PlaySound(sounds[7]);
        pause = true;
        isNeckReverse = false;
        ScibidiAnimation.AnimationName = "lose";
        lr.enabled = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<EnemyCamera>().playerLose();
        }
        uiScript.lose();
        Invoke("Dying", 0.9f);

    }
    private void Winning()
    {
        PlaySound(sounds[4]);
        PlaySound(sounds[5]);
        pause = true;
        isNeckReverse = false;
        ScibidiAnimation.AnimationName = "win_loop";
        Confetti = GameObject.FindGameObjectWithTag("Confetti");
        Confetti.GetComponent<ParticleSystem>().Play();
        Sparks.SetActive(true);
        Invoke("winUIScript", 1);
    }
    private void reverseNeck()
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
                ScibidiHead.transform.transform.eulerAngles = new Vector3(0, 0, rotatesZ[rotatesZ.Count - 1]);
                ScibidiHead.transform.position = new Vector2(_startShX, _startShY);
                positions.Clear();
                positions.Add(new Vector3(_startShX, _startShY - 0.4f, 0));
                positions.Add(new Vector3(_startShX, _startShY - 0.2f, 0));
                positions.Add(new Vector3(_startShX, _startShY, 0));
                lr.SetPositions(positions.ToArray());
                Losing();
            }
            else if (win)
            {
                ScibidiHead.transform.transform.eulerAngles = new Vector3(0, 0, rotatesZ[rotatesZ.Count - 1]);
                ScibidiHead.transform.position = new Vector2(_startShX, _startShY);
                positions.Clear();
                positions.Add(new Vector3(_startShX, _startShY - 0.4f, 0));
                positions.Add(new Vector3(_startShX, _startShY - 0.2f, 0));
                positions.Add(new Vector3(_startShX, _startShY, 0));
                lr.SetPositions(positions.ToArray());
                Winning();
            }
            else
            {
                isNeckReverse = false;
                GameObject.Find("ScibidiHeadPivot").GetComponent<Player>().inNeckReverse = false;
            }


        
    }
    private void moveHead()
    {
        shX = ScibidiHead.transform.position.x;
        shY = ScibidiHead.transform.position.y;
        counter++;
        if(positions.Last().x == shX && positions.Last().y == shY)
        {
            return;
        }
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

        ScibidiAnimation.AnimationName = "fly";
    }
    private void FixedUpdate()
    {
        if (pause) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && !win)
        {
            startReverseNeck("win");
        }
       
        if (isNeckReverse)
        {
            reverseNeck();
        }
        else
        {
            moveHead();
        }
        
    }
  
    public void startReverseNeck(string gameEnd)
    {
        Debug.Log("STARTREVERSENECK");
        PlaySound(sounds[3]);
        if(gameEnd == "lose")
        {
            lose = true;
            isNeckReverse = true;

        }
        else if (gameEnd == "win")
        {
            SaveManager.SaveState();
            GameObject.Find("ScibidiHeadPivot").GetComponent<Player>().inNeckReverse = true;
            win = true;
            isNeckReverse = true;
        }
        else if (gameEnd == "nothing")
        {
            isNeckReverse = true;
        }

    }

}
