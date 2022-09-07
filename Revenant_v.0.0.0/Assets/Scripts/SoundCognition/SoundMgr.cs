using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Enemy가 플레이어 인지를 돕는 SoundEntity의 생성을 오브젝트 풀링 기법으로 생성하는 클래스입니다.
/// </summary>
public class SoundMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject p_SoundPrefab;
    public int p_PullingLimit = 10;


    // Member Variables
    private List<SoundEntity> m_PulledSoundList;
    private int m_Idx = 0;
    
    
    // Constructors
    private void Awake()
    {
        m_PulledSoundList = new List<SoundEntity>();
        
        if (ReferenceEquals(p_SoundPrefab, null))
        {
            Debug.Log("ERR : SoundMgr에서 풀링할 오브젝트가 없습니다.");
        }

        // SoundEntity 생성 후 초기화 후 싹 비활성화
        for (int i = 0; i < p_PullingLimit; i++)
        {
            var instance = Instantiate(p_SoundPrefab).GetComponent<SoundEntity>();
            instance.gameObject.transform.parent = transform;
            m_PulledSoundList.Add(instance);
            instance.gameObject.SetActive(false);
        }
    }
    
    
    // Functions

    /// <summary>
    /// 원하는 위치에 Sound를 생성합니다.
    /// </summary>
    /// <param name="_pos">생성될 위치</param>
    public void MakeSound(Vector2 _soundSpawnPos, bool _isPlayers, SOUNDTYPE _soundType,
        float _sizeX = 1f, float _sizeY = 0.5f, float _lifeTime = 0.5f)
    {
        var instance = m_PulledSoundList[m_Idx];
        instance.gameObject.SetActive(false);
        instance.gameObject.SetActive(true);

        instance.InitSound(new SoundHotBoxParam(_soundSpawnPos, _isPlayers, _soundType,
            new Vector2(_sizeX, _sizeY), _lifeTime));

        m_Idx++;

        if (m_Idx >= p_PullingLimit)
            m_Idx = 0;
    }
}