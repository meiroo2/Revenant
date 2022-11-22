using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialog : MonoBehaviour
{
	[field: SerializeField] public TextMeshProUGUI TextUI { get; set; }
	private string textString;
	private float currentTextCount = 0;

	public Action SkipEvent;
	public float TypingSpeed = 0.5f;
	public bool isTextEnd { get; set; } = false;
	void Start()
	{
		TextUI = GetComponentInChildren<TextMeshProUGUI>();
		TextUI.text = "";
		SkipEvent += SkipText;
	}

	// Update is called once per frame
	void Update()
	{
		transform.localScale = transform.parent.parent.localScale * 0.01f;

		if (textString.Length > 0 && currentTextCount < textString.Length - 1)
		{
			currentTextCount += Time.deltaTime * TypingSpeed;
			if (textString[(int)currentTextCount] == '<')
			{
				while (textString[(int)currentTextCount] != '>')
				{
					currentTextCount++;
				}
			}
			TextUI.text = textString.Substring(0, (int)currentTextCount + 1);
			TextUI.text += "<alpha=#00>";
			TextUI.text += textString.Substring((int)currentTextCount + 1, textString.Length - (int)currentTextCount - 1);
		}
		else
		{
			isTextEnd = true;
		}
	}

	private void SkipText()
	{
		if (currentTextCount < textString.Length - 1)
		{
			currentTextCount = textString.Length - 1;
			TextUI.text = textString;
		}
		isTextEnd = true;
	}

	public void SetDialogText(string text)
	{
		textString = text;
		currentTextCount = 0;
		isTextEnd = false;

		if(TextUI)
		TextUI.text = "";
	}

	public void SetDialogActive(bool value)
	{
		transform.localScale = transform.parent.parent.localScale * 0.01f;
		gameObject.SetActive(value);
	}
}
