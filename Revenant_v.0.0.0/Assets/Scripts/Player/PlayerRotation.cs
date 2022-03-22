using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // Visible Member Variables
    public bool m_doRotate = true;
    public bool m_spriteChangeMode = false;
    public float m_rotationHighLimitAngle = 65f;
    public float m_rotationLowLimitAngle = -45f;

    [Space(10f)]
    [Header("Up to Down")]
    public SpriteRenderer m_HeadspriteRenderer;
    public Sprite[] m_HeadSprites;

    [Space(10f)]
    public SpriteRenderer m_JacketLSpriteRenderer;
    public SpriteRenderer m_JacketRSpriteRenderer;
    public Sprite[] m_JacketLSprites;
    public Sprite[] m_JacketRSprites;

    [Space(10f)]
    public Transform m_HeadIKPos;

    // Member Variables
    private PlayerSoundnAni m_playerSoundnAni;
    private Player m_Player;

    private Vector3 mousePos;
    private Quaternion toRotation;
    private float dy;
    private float dx;
    private float rotateDegree;

    private float m_HeadSpritesDegree;
    private float m_JacketSpritesDegree;

    private Vector2 m_OriginalHeadIKPos;

    // Constructors
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
        m_playerSoundnAni = GetComponentInParent<PlayerSoundnAni>();

        if (m_spriteChangeMode == true)
            m_HeadspriteRenderer.gameObject.GetComponent<Animator>().enabled = false;

        m_HeadSpritesDegree = 180f / m_HeadSprites.Length;
        m_JacketSpritesDegree = 180f / m_JacketLSprites.Length;

        m_OriginalHeadIKPos = m_HeadIKPos.localPosition;
    }

    // Updates
    private void Update()
    {
        if (m_doRotate)
        {
            if (rotateDegree > 90f || rotateDegree < -90f)
            {
                if (m_Player.m_isRightHeaded)
                {
                    m_Player.setisRightHeaded(false);
                }
                else
                {
                    m_Player.setisRightHeaded(true);
                }

                m_playerSoundnAni.playplayerAnim();
            }

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            dy = mousePos.y - transform.position.y;
            dx = mousePos.x - transform.position.x;

            if (!m_Player.m_isRightHeaded)
            {
                dy = -dy;
                dx = -dx;
            }

            rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

            if (m_Player.m_isRightHeaded)
            {
                if(rotateDegree < m_rotationHighLimitAngle && rotateDegree > m_rotationLowLimitAngle)
                    toRotation = Quaternion.Euler(0f, 0f, rotateDegree);
                else
                {
                    if (rotateDegree > 0)
                        toRotation = Quaternion.Euler(0f, 0f, m_rotationHighLimitAngle);
                    else
                        toRotation = Quaternion.Euler(0f, 0f, m_rotationLowLimitAngle);
                }

                // 오른쪽 보고
                if (m_OriginalHeadIKPos.x - rotateDegree / 400f >= -0.10f && m_OriginalHeadIKPos.x - rotateDegree / 400f <= 0.14f)
                    m_HeadIKPos.localPosition = new Vector2(m_OriginalHeadIKPos.x - rotateDegree / 400f, m_OriginalHeadIKPos.y);
            }
            else if(!m_Player.m_isRightHeaded)
            {
                if (-rotateDegree < m_rotationHighLimitAngle && -rotateDegree > m_rotationLowLimitAngle)
                    toRotation = Quaternion.Euler(0f, 0f, rotateDegree);
                else
                {
                    if (-rotateDegree > 0)
                        toRotation = Quaternion.Euler(0f, 0f, -m_rotationHighLimitAngle);
                    else
                        toRotation = Quaternion.Euler(0f, 0f, -m_rotationLowLimitAngle);
                }

                // 왼쪽 보고
                if (m_OriginalHeadIKPos.x + rotateDegree / 400f >= -0.10f && m_OriginalHeadIKPos.x + rotateDegree / 400f <= 0.14f)
                    m_HeadIKPos.localPosition = new Vector2(m_OriginalHeadIKPos.x + rotateDegree / 400f, m_OriginalHeadIKPos.y);
            }

            if (m_spriteChangeMode)
            {
                if (m_Player.m_isRightHeaded)
                {
                    float Tempdegree = 90f;
                    int SpriteNum = m_HeadSprites.Length - 1;
                    do
                    {
                        if (Tempdegree - m_HeadSpritesDegree >= -90f)
                        {
                            Tempdegree -= m_HeadSpritesDegree;
                        }
                        else
                        {
                            break;
                        }

                        if (rotateDegree > Tempdegree)
                        {
                            m_HeadspriteRenderer.sprite = m_HeadSprites[SpriteNum];
                            SpriteNum--;
                        }
                    } while (true);

                    Tempdegree = 90f;
                    SpriteNum = m_JacketLSprites.Length - 1;
                    do
                    {
                        if (Tempdegree - m_JacketSpritesDegree >= -90f)
                        {
                            Tempdegree -= m_JacketSpritesDegree;
                        }
                        else
                        {
                            break;
                        }

                        if (rotateDegree > Tempdegree)
                        {
                            m_JacketLSpriteRenderer.sprite = m_JacketLSprites[SpriteNum];
                            m_JacketRSpriteRenderer.sprite = m_JacketRSprites[SpriteNum];
                            SpriteNum--;
                        }
                    } while (true);
                }
                else
                {
                    float Tempdegree = 90f;
                    int SpriteNum = m_HeadSprites.Length - 1;
                    do
                    {
                        if (Tempdegree - m_HeadSpritesDegree >= -90f)
                        {
                            Tempdegree -= m_HeadSpritesDegree;
                        }
                        else
                        {
                            break;
                        }

                        if (-rotateDegree > Tempdegree)
                        {
                            m_HeadspriteRenderer.sprite = m_HeadSprites[SpriteNum];
                            SpriteNum--;
                        }
                    } while (true);

                    Tempdegree = 90f;
                    SpriteNum = m_JacketLSprites.Length - 1;
                    do
                    {
                        if (Tempdegree - m_JacketSpritesDegree >= -90f)
                        {
                            Tempdegree -= m_JacketSpritesDegree;
                        }
                        else
                        {
                            break;
                        }

                        if (-rotateDegree > Tempdegree)
                        {
                            m_JacketLSpriteRenderer.sprite = m_JacketLSprites[SpriteNum];
                            m_JacketRSpriteRenderer.sprite = m_JacketRSprites[SpriteNum];
                            SpriteNum--;
                        }
                    } while (true);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (m_doRotate)
        {
            transform.rotation = toRotation;
        }
    }
}
