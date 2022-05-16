using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoiseType
{
    WALK,
    FIREARM
}
public class NoisePrefab : MonoBehaviour
{
    // Visible Member Variables
    public bool m_isPlayer = true;
    public NoiseType m_NoiseType = NoiseType.WALK;
    private BoxCollider2D m_NoiseBoxcollider;

    // Member Variables
    public int m_curLayer { get; set; }
    public int m_curRoom { get; set; }
    public int m_curFloor { get; set; }
    public Vector2 m_curPos { get; set; }
    public LocationInfo m_curLocation;

    // Constructors
    private void Awake()
    {
        m_NoiseBoxcollider = GetComponent<BoxCollider2D>();
    }

    // Updates


    // Physics


    // Functions
    public void InstantiateNoise(NoiseType _noiseType, Vector2 _noiseSize, LocationInfo _noiseLocation, bool _isPlayer)
    {
        m_NoiseType = _noiseType;
        m_NoiseBoxcollider.size = _noiseSize;
        m_isPlayer = _isPlayer;

        m_curLocation = _noiseLocation;

        transform.position = m_curPos;


        Invoke(nameof(EndNoise), 0.5f);
    }

    // 기타 분류하고 싶은 것이 있을 경우
    private void EndNoise() {
        m_NoiseBoxcollider.size = Vector2.zero;
        gameObject.SetActive(false); 
    }
}
