using UnityEngine;


public class Shield : MonoBehaviour, IHotBox
{
    // Visible Member Variabels
    [field: SerializeField] public int m_Hp { get; private set; } = 30;
    
    
    // Member Variables
    public delegate void SuperArmorDelegate();
    public SuperArmorDelegate m_Callback = null;
    
    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = true;
    
    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        m_Hp -= _param.m_Damage;
        
        if(m_Hp <= 0)
            gameObject.SetActive(false);
        
        return 1;
    }
}