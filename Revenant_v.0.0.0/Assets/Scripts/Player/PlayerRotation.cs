using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // Member Variables
    public bool m_doRotate = true;
    public bool m_spriteChangeMode = false;
    public SpriteRenderer m_HeadspriteRenderer;
    public Sprite[] m_HeadSprites;

    private PlayerSoundnAni m_playerSoundnAni;
    private Player m_Player;

    private Vector3 mousePos;
    private Quaternion toRotation;
    private float dy;
    private float dx;
    private float rotateDegree;

    // Constructors
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
        m_playerSoundnAni = GetComponentInParent<PlayerSoundnAni>();

        if (m_spriteChangeMode == true)
            m_HeadspriteRenderer.gameObject.GetComponent<Animator>().enabled = false;
    }

    // Updates
    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        dy = mousePos.y - transform.position.y;
        dx = mousePos.x - transform.position.x;

        if (m_Player.m_isRightHeaded == false)
        {
            dy = -dy;
            dx = -dx;
        }

        rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        toRotation = Quaternion.Euler(0f, 0f, rotateDegree);


        if (m_doRotate && rotateDegree > 90f || rotateDegree < -90f)
        {
            if (m_Player.m_isRightHeaded)
            {
                m_Player.setisRightHeaded(false);
            }
            else
            {
                m_Player.setisRightHeaded(true);
            }

            if (!m_spriteChangeMode)
                m_playerSoundnAni.playplayerAnim();
        }

        if (m_spriteChangeMode)
        {
            if (m_Player.m_isRightHeaded)
            {
                if (rotateDegree > 30f)
                    m_HeadspriteRenderer.sprite = m_HeadSprites[0];
                else if (rotateDegree < -30f)
                    m_HeadspriteRenderer.sprite = m_HeadSprites[2];
                else
                    m_HeadspriteRenderer.sprite = m_HeadSprites[1];
            }
            else
            {
                if (-rotateDegree > 30f)
                    m_HeadspriteRenderer.sprite = m_HeadSprites[0];
                else if (-rotateDegree < -30f)
                    m_HeadspriteRenderer.sprite = m_HeadSprites[2];
                else
                    m_HeadspriteRenderer.sprite = m_HeadSprites[1];
            }
        }
    }
    private void FixedUpdate()
    {
        if (m_doRotate)
            transform.rotation = toRotation;
    }
}
