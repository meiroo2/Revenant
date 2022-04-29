using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calcuator : MonoBehaviour
{
    delegate float Calculator(float a, float b);
    Calculator onCalculate;  // ��������Ʈ ������Ʈ ����

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
            Debug.Log("����� : " + onCalculate(1, 10));  // == Sum(1, 10);
        }
    }
}
