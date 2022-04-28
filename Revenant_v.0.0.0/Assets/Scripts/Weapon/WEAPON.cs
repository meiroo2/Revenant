using UnityEditor;
using UnityEngine;


public class WEAPON : MonoBehaviour
{
    // Visible Member Variables
    public GameObject m_bulletPrefab;
    public bool m_isPlayers { get; protected set; }

    // Member Variables


    // Constructors


    // Updates


    // Physics


    // Functions
    public virtual int Fire() { return 0; } // 0 = Fail, 1 = Success, 2 = Fail(No Ammo)
    public virtual int Reload() { return 0; }   // 0 = Fail, 1 = Success

    // 기타 분류하고 싶은 것이 있을 경우
}