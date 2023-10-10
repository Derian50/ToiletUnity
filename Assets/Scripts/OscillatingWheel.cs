using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OscillatingWheel : AbstractMultiplierWheel
{
    private const float MinAngle = -85, MaxAngle = 85;
    
    /// <summary>
    /// Coefficients in function transforming time to a Value on the wheel 
    /// </summary>
    protected float _angle;
    protected float _angleOffset;
    protected bool _dir;
    
    public override event Action<int> OnWheelValueChanged;

    
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
            }
        }
    }

    public override void StartSpinning()
    {
        base.StartSpinning();

        _angle = Random.Range(MinAngle, MaxAngle);
        _valueI = CalculateI(_angle);
        OnWheelValueChanged?.Invoke(_values[_valueI]);
    }


    private int CalculateI(float angle)
    {
        switch (angle)
        {
            case > -90f and <= -60f:
                return 0;
            case > -60f and <= -15f:
                return 1;
            case > -15f and <= 35f:
                return 2;
            case > 35f and <= 90f:
                return 3;
        }
        return -1;
    }
}
