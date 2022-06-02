using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// https://robotree.tistory.com/7
// �� ȸ���� ���� ��ũ��Ʈ(�÷��̾�&�� ����)
public class RotationObj : MonoBehaviour
{

    // Member Variables
    public GameObject Player;    // ȸ���ϴ� ����� �θ� ������Ʈ
    public bool doRotate = true; // true���� �۵�
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
    public void Update()    // �⺻������ LookAt() �Լ��� �������� ������ ��(�ڼ��� ������ ��ũ ����)
                            // Enemy �÷��װ� ���� ���� ���, target�� ��ǥ�� ��� ������ Player�� ����
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