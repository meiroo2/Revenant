using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperArmorMgr : MonoBehaviour
{
    public float p_MaxScaleRatio = 1.5f;
    public float p_SuperArmorSpeed = 20f;
    public float p_DistanceBetweenOriginPos = 0.08f;

    [Space(30f)]
    [Header("���� ����")]
    public GameObject m_ChildObj;
    private GameObject m_ParentObj;

    private Transform m_InstantiatedTransform;

    private bool m_isSuperArmorOn = false;

    private void Awake()
    {
        m_ParentObj = GetComponentInParent<SpriteRenderer>().gameObject;
        GameObject sans = Instantiate(m_ChildObj);
        sans.transform.parent = m_ParentObj.transform;
        m_InstantiatedTransform = sans.transform;
        m_InstantiatedTransform.localPosition = Vector2.zero;
        m_InstantiatedTransform.localScale = Vector2.zero;

        sans.GetComponent<SpriteRenderer>().sprite = m_ParentObj.GetComponent<SpriteRenderer>().sprite;

        sans.SetActive(false);
    }

    private void Update()
    {
        if (!m_isSuperArmorOn)
            return;

        m_InstantiatedTransform.localScale = Vector2.Lerp(m_InstantiatedTransform.localScale, Vector2.one, Time.deltaTime * p_SuperArmorSpeed);
        m_InstantiatedTransform.localPosition = Vector2.Lerp(m_InstantiatedTransform.localPosition, Vector2.zero, Time.deltaTime * p_SuperArmorSpeed);
        
        if(m_InstantiatedTransform.localScale.x <= 1.03f)
        {
            m_InstantiatedTransform.localScale = Vector2.one;
            m_InstantiatedTransform.localPosition = Vector2.zero;
            m_isSuperArmorOn = false;

            m_InstantiatedTransform.gameObject.SetActive(false);
        }
    }

    public void doSuperArmor()
    {
        if (m_isSuperArmorOn)
            return;

        m_isSuperArmorOn = true;
        m_InstantiatedTransform.gameObject.SetActive(true);
        m_InstantiatedTransform.localScale = new Vector2(p_MaxScaleRatio, p_MaxScaleRatio);
        m_InstantiatedTransform.localPosition = new Vector2(m_InstantiatedTransform.localPosition.x - p_DistanceBetweenOriginPos,
            m_InstantiatedTransform.localPosition.y + p_DistanceBetweenOriginPos);
    }
}