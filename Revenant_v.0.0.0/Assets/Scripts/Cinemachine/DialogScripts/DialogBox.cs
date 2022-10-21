using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI textMesh { get; set; }
    private string textString;
    private float currentTextCount = 0;

    public Action SkipEvent;
    public bool isTypingEffect = false;
    public float TypingSpeed = 0.5f;
    public bool isTextEnd { get; set; } = false;

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void Update()
    {
		textMesh = GetComponentInChildren<TextMeshProUGUI>();
		textMesh.rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
	}
#endif

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textString = textMesh.text;
        if(isTypingEffect)
        textMesh.text = "";
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
            textMesh.text = textString.Substring(0, (int)currentTextCount + 1);
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
		}
        else
        {
            isTextEnd = true;
		}
    }
}
