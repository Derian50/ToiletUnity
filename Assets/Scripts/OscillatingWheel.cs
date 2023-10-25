using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class OscillatingWheel : AbstractMultiplierWheel
{
    private const float MinAngle = -120, MaxAngle = 120;
    
    /// <summary>
    /// Coefficients in function transforming time to a Value on the wheel 
    /// </summary>
    protected float _angle;
    protected float _angleOffset;
    protected bool _dir;
    public int mult;
    public override event Action<int> OnWheelValueChanged;

    private void Stop()
    {
        _isSpinning = false;
    }
    private void Start()
    {
        StartSpinning();
    }
    private void Update()
    {
        if (!_isSpinning) return;

        {
            // Rotating wheel
            float t = Time.time - _spinStartedTimeStamp;
            _angle += Time.deltaTime * _angularVelocity;
            if (_angle < MinAngle)
            {
                _angularVelocity *= -1;
                _angle = MinAngle + Time.deltaTime * Mathf.Abs(_angularVelocity);
            }

            if (_angle > MaxAngle)
            {
                _angularVelocity *= -1;
                _angle = MaxAngle - Time.deltaTime * Mathf.Abs(_angularVelocity);
            }

            _arrowRoot.rotation = Quaternion.Euler(0.0f, 0.0f, _angle);
        }

        {
            // Setting multiplied value
            int I = CalculateI(_angle);
            if (I != _valueI)
            {
                _valueI = I;
                OnWheelValueChanged?.Invoke(_values[I]);
                mult = _values[I];
            }
        }
    }

    public override void StartSpinning()
    {
        base.StartSpinning();

        _angle = Random.Range(MinAngle, MaxAngle);
        _valueI = CalculateI(_angle);
        OnWheelValueChanged?.Invoke(_values[_valueI]);
        mult = _valueI;
    }


    private int CalculateI(float angle)
    {
        switch (angle)
        {
            case > -140f and <= -70f:
                return 0;
            case > -70 and <= -25f:
                return 1;
            case > -25f and <= 30:
                return 2;
            case > 30f and <= 140:
                return 3;
        }
        return -1;
    }
}
