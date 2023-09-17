using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformScript : MonoBehaviour
{
    [SerializeField] private Sprite _btnOnSprite;
    [SerializeField] private Sprite _btnOffSprite;
    [SerializeField] private bool _isMoving;
    private SpriteRenderer Sr;
    [SerializeField] private GameObject _platform;
    private float _rotationPlatform;
    private Vector3 _vector3MovingPlatform;
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log(_platform.transform.eulerAngles.z);
        _rotationPlatform = _platform.transform.eulerAngles.z;
        switch (_rotationPlatform)
        {
            case 0:
                _vector3MovingPlatform = new Vector3(0.01f, 0, 0);
                break;
            case 90:
                _vector3MovingPlatform = new Vector3(0, 0.01f, 0);
                break;
            case 180:
                _vector3MovingPlatform = new Vector3(-0.01f, 0, 0);
                break;
            case 270:
                _vector3MovingPlatform = new Vector3(0, -0.01f, 0);
                break;

        }
        Sr = GetComponent<SpriteRenderer>();
        Sr.sprite = _btnOnSprite;
    }
    private void DestroyPlatform()
    {
        _platform.SetActive(false);
    }
    private void StopMoving()
    {
        _isMoving = false;
    }
    private void StartMoving()
    {
        _isMoving = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Sr.sprite == _btnOnSprite)
        {
            Sr.sprite = _btnOffSprite;
            StartMoving();
            Invoke("StopMoving", 3);
        }
       
    }
    private void Update()
    {
        if (_isMoving)
        {
            _platform.transform.position = _platform.transform.position + _vector3MovingPlatform;
        }
    }
}
