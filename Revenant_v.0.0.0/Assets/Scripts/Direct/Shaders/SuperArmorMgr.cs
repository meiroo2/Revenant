using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SuperArmorMgr : MonoBehaviour
{
    // Visible Member Variables
    public float p_MaxScaleRatio = 1.5f;
    public float p_SuperArmorSpeed = 20f;
    public float p_DistanceBetweenOriginPos = 0.08f;
    [Space(30f)]
    [Header("¶£Áã ±ÝÁö")]
    public GameObject m_ChildObj;
    
    // Member Variables
    public delegate void SuperArmorDelegate();
    public SuperArmorDelegate m_Callback = null;

    private float m_Temp;
    protected GameObject m_ParentObj;
    protected SpriteRenderer m_ParentObjRenderer;
    protected Transform m_InstantiatedTransform;
    protected SpriteRenderer m_InstantiatedObjRenderer;
    protected bool m_isSuperArmorOn = false;

    private void Awake()
    {
        m_ParentObj = GetComponentInParent<SpriteRenderer>().gameObject;
        m_ParentObjRenderer = m_ParentObj.GetComponent<SpriteRenderer>();
        
        m_InstantiatedTransform = Instantiate(m_ChildObj, m_ParentObj.transform, true).transform;
        m_InstantiatedTransform.localPosition = Vector2.zero;
        m_InstantiatedTransform.localScale = Vector2.zero;

        m_InstantiatedObjRenderer = m_InstantiatedTransform.GetComponent<SpriteRenderer>();
        m_InstantiatedObjRenderer.sprite = m_ParentObjRenderer.sprite;

        m_InstantiatedTransform.gameObject.SetActive(false);
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

            if (m_Callback != null)
                m_Callback();

            m_Callback = null;
            
            m_InstantiatedTransform.gameObject.SetActive(false);
        }
    }

    public virtual void doSuperArmor()
    {
        if (m_isSuperArmorOn)
            return;
        
        m_InstantiatedObjRenderer.sprite = m_ParentObjRenderer.sprite;
        
        m_isSuperArmorOn = true;
        m_InstantiatedTransform.gameObject.SetActive(true);
        m_InstantiatedTransform.localScale = new Vector2(p_MaxScaleRatio, p_MaxScaleRatio);

        if (m_ParentObj.transform.localScale.x > 0)
        {
            m_InstantiatedTransform.localPosition = new Vector2(m_InstantiatedTransform.localPosition.x - p_DistanceBetweenOriginPos,
                m_InstantiatedTransform.localPosition.y + p_DistanceBetweenOriginPos);
        }
        else
        {
            m_InstantiatedTransform.localPosition = new Vector2(m_InstantiatedTransform.localPosition.x + p_DistanceBetweenOriginPos,
                m_InstantiatedTransform.localPosition.y + p_DistanceBetweenOriginPos);
        }
    }

    public void SetCallback(SuperArmorDelegate _input, bool _doReset = false)
    {
        if (_doReset)
            m_Callback = null;

        m_Callback += _input;
    }
}