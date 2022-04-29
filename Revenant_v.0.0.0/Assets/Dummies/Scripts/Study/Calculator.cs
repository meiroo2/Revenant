using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calcuator : MonoBehaviour
{
    delegate float Calculator(float a, float b);
    Calculator onCalculate;  // 델리게이트 오브젝트 생성

    void Start()
    {
        onCalculate = Sum;
        onCalculate += Substract;
        onCalculate += Multiply;
    }

    public float Sum(float a, float b)
    {
        Debug.Log(a + b);
        return a + b;
    }

    public float Substract(float a, float b)
    {
        Debug.Log(a - b);
        return a - b;
    }

    public float Multiply(float a, float b)
    {
        Debug.Log(a * b);
        return a * b;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("결과값 : " + onCalculate(1, 10));  // == Sum(1, 10);
        }
    }
}
