using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typingeffect : MonoBehaviour
{
    public Text tx;
    private string m_text = "Osolka Station Exit 2";

    void Start()
    {
        StartCoroutine(_typing()); 
    }

IEnumerator _typing()
    {
        yield return new WaitForSeconds(1.5f);
        for(int i =0; i <= m_text.Length; i++)
        {
            tx.text = m_text.Substring(0,i);

            yield return new WaitForSeconds(0.02f);
        }
    }
 
}
