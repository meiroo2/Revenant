using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// https://robotree.tistory.com/7
// 팔 회전을 위한 스크립트(플레이어&적 공통)
public class RotationObj : MonoBehaviour
{

    // Member Variables
    public GameObject Player;    // 회전하는 대상의 부모 오브젝트
    public bool doRotate = true; // true여야 작동
    public bool doRotateInstant = false;
    public bool doFlip = false;
    public float m_RotationSpeed = 1f;

    public Sprite[] m_HeadSprites;
    public bool doSpriteChange = false;
    private SpriteRenderer spriteRenderer;

    private Player m_Player;

    private Vector3 mousePos;
    private Quaternion toRotation;
    private float dy;
    private float dx;
    private float rotateDegree;


    // Constructor
    private void Awake()
    {
        m_Player = Player.GetComponent<Player>();

        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Getter

    // Setter

    // Basic Functions
    public void Update()    // 기본적으로 LookAt() 함수를 수동으로 구현한 것(자세한 내용은 링크 참조)
                            // Enemy 플래그가 켜져 있을 경우, target의 좌표를 멤버 변수인 Player로 설정
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        dy = mousePos.y - transform.position.y;
        dx = mousePos.x - transform.position.x;

        if (m_Player.m_IsRightHeaded == false)
        {
            dy = -dy;
            dx = -dx;
        }

        rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        toRotation = Quaternion.Euler(0f, 0f, rotateDegree);

        if (doFlip)
        {
            if (rotateDegree > 90f || rotateDegree < -90f)
            {
                if (m_Player.m_IsRightHeaded)
                    m_Player.setisRightHeaded(false);
                else
                    m_Player.setisRightHeaded(true);
            }
        }

        if (doSpriteChange)
        {
            if (m_Player.m_IsRightHeaded)
            {
                if (rotateDegree > 30f)
                    spriteRenderer.sprite = m_HeadSprites[0];
                else if (rotateDegree < -30f)
                    spriteRenderer.sprite = m_HeadSprites[2];
                else
                    spriteRenderer.sprite = m_HeadSprites[1];
            }
            else
            {
                if (-rotateDegree > 30f)
                    spriteRenderer.sprite = m_HeadSprites[0];
                else if (-rotateDegree < -30f)
                    spriteRenderer.sprite = m_HeadSprites[2];
                else
                    spriteRenderer.sprite = m_HeadSprites[1];
            }
        }
    }

    private void FixedUpdate()
    {
        if (doRotate)
        {
            if (!doRotateInstant)
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * m_RotationSpeed);
            else if (doRotateInstant)
                transform.rotation = toRotation;
        }
    }
}