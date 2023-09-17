using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserScript : MonoBehaviour
{
    [SerializeField] private Sprite _btnOnSprite;
    [SerializeField] private Sprite _btnOffSprite;
    [SerializeField] private bool _isEnabled;
    private SpriteRenderer Sr;
    [SerializeField] private GameObject _ray;
    [SerializeField] private GameObject _mainLaser;
    [SerializeField] private float _movingIntervalX;
    private float _startX;
    private float _currentX;
    private bool _reverseMove = false;
    // Start is called before the first frame update
    private void Start()
    {
        _startX = _mainLaser.transform.position.x;
        _currentX = _startX;
        Sr = GetComponent<SpriteRenderer>();
        SetState();
    }
    private void SetState()
    {
        if (_isEnabled)
        {
            _ray.SetActive(true);
            Sr.sprite = _btnOnSprite;
        }
        else
        {
            _ray.SetActive(false);
            Sr.sprite = _btnOffSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isEnabled = !_isEnabled;
        SetState();
    }
    private void Update()
    {
        _currentX = _mainLaser.transform.position.x;
        if (_movingIntervalX > 0)
        {
            if (_currentX >= _startX + _movingIntervalX)
            {
                _reverseMove = true;
            }
            else if(_currentX <= _startX)
            {
                _reverseMove = false;
            }
            if (_reverseMove)
            {
                _mainLaser.transform.position += new Vector3((float)(_movingIntervalX / -600), 0, 0);
                _ray.transform.position += new Vector3((float)(_movingIntervalX / -600), 0, 0);
            }
            else
            {
                _mainLaser.transform.position += new Vector3((float)(_movingIntervalX / 600), 0, 0);
                _ray.transform.position += new Vector3((float)(_movingIntervalX / 600), 0, 0);
            }
            
        }
    }
}
