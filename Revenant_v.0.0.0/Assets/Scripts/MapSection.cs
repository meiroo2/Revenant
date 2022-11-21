using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSection : MonoBehaviour
{
    private int SectionNum = 0;

    private List<MapSection> MapSections;
    private Player _player;

    private void Awake()
    {
        MapSections = FindObjectsOfType<MapSection>().ToList();
    }

    void Start()
    {
        _player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        for (int i = 0; i < MapSections.Count; i++)
        {
            MapSections[i].SectionNum = i + 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _player.PlayerMapSectionNum = SectionNum;
        }
        
        if (col.TryGetComponent(out BasicEnemy basicEnemy))
        {
            basicEnemy.EnemyMapSectionNum = SectionNum;
        }
    }
}
