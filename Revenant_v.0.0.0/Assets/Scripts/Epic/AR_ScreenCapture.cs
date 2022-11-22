using System;
using UnityEngine;
using UnityEngine.UI;


public class AR_ScreenCapture : MonoBehaviour
{
    public float m_Size = 10f;
    public SpriteRenderer m_LImg;
    public SpriteRenderer m_RImg;
    public SpriteRenderer m_BlackBackSprite;
    public SpriteRenderer m_LineSpriteForEffect;

    private Camera m_MainCam;

    public Vector2 m_LineOriginScale { get; private set; }

    private void Awake()
    {
        m_LineOriginScale = m_LineSpriteForEffect.transform.localScale;
        
        m_BlackBackSprite.gameObject.SetActive(false);
        m_LineSpriteForEffect.gameObject.SetActive(false);
        m_MainCam = Camera.main;
    }

    private void Start()
    {
        transform.localPosition = new Vector3(0f, 0f, 10f);
        transform.localScale = new Vector3(m_MainCam.orthographicSize / m_Size,
            m_MainCam.orthographicSize / m_Size, 1f);
    }
}