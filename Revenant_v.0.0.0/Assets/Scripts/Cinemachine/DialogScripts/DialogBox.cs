using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [field: SerializeField] public Text TextUI { get; set; }
    private string textString;
    private float currentTextCount = 0;

    public Action SkipEvent;
    public bool isTypingEffect = false;
    public float TypingSpeed = 0.5f;
    public bool isOnPlayerPosition = false;
    public bool isTextEnd { get; set; } = false;



#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void Update()
    {
		TextUI = GetComponentInChildren<Text>();
		//TextUI.rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
	}
#endif

    void Start()
    {
        TextUI = GetComponentInChildren<Text>();
        textString = TextUI.text;
        if(isTypingEffect)
        TextUI.text = "";
        SkipEvent += SkipText;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (isTypingEffect)
        {
	    	if (currentTextCount < textString.Length - 1)
            {
    			currentTextCount += Time.deltaTime * TypingSpeed;
            }
            TextUI.text = textString.Substring(0, (int)currentTextCount + 1);
        }
        else
        {
            currentTextCount = textString.Length - 1;
		}
	}

    private void SkipText()
    {
        if(currentTextCount < textString.Length - 1)
        {
            currentTextCount = textString.Length - 1;
            TextUI.text = textString;
		}
        else
        {
            isTextEnd = true;
		}
    }
}
