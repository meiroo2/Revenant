using System;
using System.Collections.Generic;
using UnityEngine;


public class StringInput : MonoBehaviour
{
    public List<int> m_InputResult = new List<int>();

    public string m_Input = "INS 1 INS 2 INS 3 EOI";

    private void Awake()
    {
        string eng = null;
        int val = 0;
        bool isEng = true;
        
        foreach (var VARIABLE in m_Input)
        {
            if (isEng)
            {
                if (char.IsWhiteSpace(VARIABLE))
                {
                    if (eng == "DEL")
                    {
                        m_InputResult.RemoveAt(m_InputResult.Count - 1);
                        eng = null;
                    }
                    else if (eng == "EOI")
                    {
                        break;
                    }
                    else
                    {
                        isEng = false;
                    }
                }
                else
                {
                    eng += VARIABLE;   
                }
            }
            else
            {
                if (char.IsWhiteSpace(VARIABLE))
                {
                    m_InputResult.Add(val);
                    val = 0;
                    eng = null;
                    isEng = true;
                }
                else
                {
                    val += VARIABLE - 48;
                }
            }
        }

        Debug.Log("Completed.");
        foreach (var VARIABLE in m_InputResult)
        {
            Debug.Log(VARIABLE.ToString());
        }
    }
}