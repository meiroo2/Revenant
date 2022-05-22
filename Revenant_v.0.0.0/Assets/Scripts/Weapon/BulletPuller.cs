using UnityEngine;



public class BulletPuller : MonoBehaviour
{
    // Visible Member Variables
    public int p_BulletPullCount = 0;
    public GameObject p_PullingBullet;
    protected bool m_isPlayers = false;


    // Member Variables
    protected int m_Idx = 0;
    protected Bullet[] m_PulledBulletArr;
    protected HitSFXMaker m_HitSFXMaker;


    // Constructors


    // Functions
    public virtual void MakeBullet(bool _isRightHeaded, Vector2 _Position, Quaternion _Rotation, float _Speed,
        int _damage)
    {
    }
}