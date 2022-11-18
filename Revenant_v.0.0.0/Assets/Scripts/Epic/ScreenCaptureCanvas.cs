using System;
using UnityEngine;
using UnityEngine.UI;


public class ScreenCaptureCanvas : MonoBehaviour
{
    public Image m_LImg;
    public Image m_RImg;
    public SpriteRenderer m_BlackBackSprite;
    public SpriteRenderer m_LineSpriteForEffect;

    public Vector2 m_LineOriginScale { get; private set; }

    private void Awake()
    {
        m_LineOriginScale = m_LineSpriteForEffect.transform.localScale;
        
        m_BlackBackSprite.gameObject.SetActive(false);
        m_LineSpriteForEffect.gameObject.SetActive(false);
    }

    private void Start()
    {
        var canvas = GetComponent<Canvas>();
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 10;
    }
}