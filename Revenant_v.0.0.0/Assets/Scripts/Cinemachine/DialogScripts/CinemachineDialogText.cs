using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CinemachineDialogText : MonoBehaviour
{
	[field: SerializeField] public TextMeshProUGUI TextUI { get; set; }
	private string textString;
	private float currentTextCount = 0;

	public bool isTypingEffect = false;
	public float TypingSpeed = 0.5f;
	public bool isTextEnd { get; set; } = false;





	void Start()
	{
		TextUI = GetComponentInChildren<TextMeshProUGUI>();
		textString = TextUI.text;
		if (isTypingEffect)
			TextUI.text = "";

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (isTypingEffect)
		{
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
			}
		}
		else
		{
			currentTextCount = textString.Length - 1;
		}
	}
}
