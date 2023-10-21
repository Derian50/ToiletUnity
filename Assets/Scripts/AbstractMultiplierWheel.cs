using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AbstractMultiplierWheel : MonoBehaviour
{
    [SerializeField] protected Transform _arrowRoot;
    [SerializeField] protected TextMeshProUGUI[] _valueTexts;
    
    protected int[] _values;
    protected float _period;
    protected float _angularVelocity;
    
    protected bool _isSpinning;
    protected float _spinStartedTimeStamp;
    protected int _valueOffset;
    protected int _valueI;
    
    public abstract event Action<int /*Wheel Value*/> OnWheelValueChanged;

    protected virtual void Awake()
    {
        _values = new int[4] { 4, 3, 2, 1 };
        _period = 1f;
        _angularVelocity = 360f / _period;
        // _b is defined in moment of starting to spin the wheel
    }
    
    private void Start()
    {
        if (_valueTexts.Length != _values.Length)
            throw new Exception("Number of segments on the wheel should correspond to number of values in config!!!");

        for (int i = 0; i < _values.Length; i++)
            _valueTexts[i].text = $"x{_values[i]}";
    }

    public virtual void StartSpinning()
    {
        _isSpinning = true;
        _spinStartedTimeStamp = Time.fixedTime;
        _valueOffset = Random.Range(0, _values.Length);
        _valueI = _valueOffset;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> A value on a wheel where the arrow stopped </returns>
    public virtual int StopSpinning()
    {
        _isSpinning = false;
        return _values[_valueI];
    }
}
