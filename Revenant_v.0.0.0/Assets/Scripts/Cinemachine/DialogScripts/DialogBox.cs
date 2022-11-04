using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
	[field: SerializeField] public Text TextUI { get; set; }
	private string textString;

	public Action SkipEvent;
	public bool isTypingEffect = false;
	public bool isOnPlayerPosition = false;
	public bool isTextEnd { get; set; } = false;





	void Start()
	{
		SkipEvent += SkipText;
	}

	private void OnEnable()
	{
		TextUI = GetComponentInChildren<Text>();
		if (enableTyping || isTypingEffect)
		{
			textString = TextUI.text;
			TextTyping(TextUI.text);
		}
		else
		{
			TextUI.text = TextUI.text;
		}
	}

	private void SkipText()
	{
		StopAllCoroutines();
		if (isTypingEffect && isTyping)
		{
			isTyping = false;
			TextUI.text = textString;
		}
		else if (!isTyping)
		{
			isTextEnd = true;
		}


		Debug.Log("d");
	}


	private string inputText;

	[SerializeField]
	private float typingSpeed = 5;

	[SerializeField]
	private bool enableTyping;

	private bool isTyping;

	private readonly string[] richTextSymbols = { "b", "i" };
	private readonly string[] richTextCloseSymbols = { "b", "i", "size", "color" };

	public bool IsTyping => isTyping;
	public void TextTyping(string text)
	{
		if (isTyping)
		{
			StopAllCoroutines();
			isTyping = false;
		}

		StartCoroutine(CoTextTyping(text));
	}
	private IEnumerator CoTextTyping(string text)
	{
		isTyping = true;

		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

		int typingIndex = 0;
		float nowWaitTime = 0f;
		char[] splitTypingTexts = text.ToCharArray();
		StringBuilder stringBuilder = new StringBuilder();

		bool bold = false;
		bool italic = false;
		bool size = false;
		bool color = false;
		int fontSize = 0;
		string colorCode = null;

		inputText = text;
		TextUI.text = null;

		while (true)
		{
			yield return waitForEndOfFrame;
			nowWaitTime += Time.deltaTime;

			if (typingIndex == splitTypingTexts.Length)
			{
				break;
			}

			if (nowWaitTime >= typingSpeed)
			{
				if (TextUI.supportRichText)
				{
					bool symbolCatched = false;

					for (int i = 0; i < richTextSymbols.Length; i++)
					{
						string symbol = string.Format("<{0}>", richTextSymbols[i]);
						string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

						if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextSymbols[i].Length) < text.Length && text.Substring(typingIndex, 2 + richTextSymbols[i].Length).Equals(symbol))
						{
							switch (richTextSymbols[i])
							{
								case "b":
									typingIndex += symbol.Length;
									if (typingIndex + closeSymbol.Length + 3 <= text.Length)
									{
										if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
										{
											bold = true;
											symbolCatched = true;
										}
									}
									break;
								case "i":
									typingIndex += symbol.Length;
									if (typingIndex + closeSymbol.Length + 3 <= text.Length)
									{
										if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
										{
											italic = true;
											symbolCatched = true;
										}
									}
									break;
							}
						}
					}

					if (splitTypingTexts[typingIndex] == '<' && typingIndex + 14 < text.Length && text.Substring(typingIndex, 8).Equals("<color=#") && splitTypingTexts[typingIndex + 14] == '>')
					{
						string closeSymbol = string.Format("</{0}>", "color");
						string tempColorCode = text.Substring(typingIndex + 8, 6);

						typingIndex += 15;
						if (typingIndex + closeSymbol.Length <= text.Length)
						{
							if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
							{
								color = true;
								colorCode = tempColorCode;
								symbolCatched = true;
							}
						}
					}

					if (splitTypingTexts[typingIndex] == '<' && typingIndex + 6 < text.Length && text.Substring(typingIndex, 6).Equals("<size="))
					{
						string closeSymbol = string.Format("</{0}>", "size");
						string sizeSub = text.Substring(typingIndex + 6);
						int symbolIndex = sizeSub.IndexOf('>');
						string tempSize = sizeSub.Substring(0, symbolIndex);

						typingIndex += 7 + tempSize.Length;
						if (typingIndex + closeSymbol.Length <= text.Length)
						{
							if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
							{
								size = true;
								fontSize = int.Parse(tempSize);
								symbolCatched = true;
							}
						}
					}

					bool closeSymbolCatched = false;

					for (int i = 0; i < richTextCloseSymbols.Length; i++)
					{
						string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

						if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextCloseSymbols[i].Length) < text.Length && text.Substring(typingIndex, 3 + richTextCloseSymbols[i].Length).Equals(closeSymbol))
						{
							switch (richTextCloseSymbols[i])
							{
								case "b":
									typingIndex += closeSymbol.Length;
									bold = false;
									closeSymbolCatched = true;
									break;
								case "i":
									typingIndex += closeSymbol.Length;
									italic = false;
									closeSymbolCatched = true;
									break;
								case "size":
									typingIndex += closeSymbol.Length;
									size = false;
									fontSize = 0;
									closeSymbolCatched = true;
									break;
								case "color":
									typingIndex += closeSymbol.Length;
									color = false;
									colorCode = null;
									closeSymbolCatched = true;
									break;
							}
						}

						if (closeSymbolCatched)
						{
							break;
						}
					}

					if (symbolCatched || closeSymbolCatched)
					{
						continue;
					}

					if (typingIndex < text.Length)
					{
						string convertedRichText = RichTextConvert(splitTypingTexts[typingIndex].ToString(), bold, italic, size, fontSize, color, colorCode);
						stringBuilder.Append(convertedRichText);
					}
				}
				else
				{
					if (typingIndex < text.Length)
					{
						stringBuilder.Append(splitTypingTexts[typingIndex]);
					}
				}

				TextUI.text = stringBuilder.ToString();
				typingIndex++;
				nowWaitTime = 0f;
			}
		}

		//타이핑이 끝났을 때 최초 입력 값으로 적용시키기
		//this.text.text = inputText;

		isTyping = false;
	}

	private string RichTextConvert(string text, bool bold, bool italic, bool size, int fontSize, bool color, string colorCode)
	{
		StringBuilder stringBuilder = new StringBuilder();

		if (bold)
		{
			stringBuilder.Append("<b>");
			stringBuilder.Append(text);
			stringBuilder.Append("</b>");
			text = stringBuilder.ToString();
			stringBuilder.Clear();
		}

		if (italic)
		{
			stringBuilder.Append("<i>");
			stringBuilder.Append(text);
			stringBuilder.Append("</i>");
			text = stringBuilder.ToString();
			stringBuilder.Clear();
		}

		if (color)
		{
			stringBuilder.Append("<color=#");
			stringBuilder.Append(colorCode);
			stringBuilder.Append(">");
			stringBuilder.Append(text);
			stringBuilder.Append("</color>");
			text = stringBuilder.ToString();
			stringBuilder.Clear();
		}

		if (size)
		{
			stringBuilder.Append("<size=");
			stringBuilder.Append(fontSize);
			stringBuilder.Append(">");
			stringBuilder.Append(text);
			stringBuilder.Append("</size>");
			text = stringBuilder.ToString();
			stringBuilder.Clear();
		}

		return text;
	}
}
