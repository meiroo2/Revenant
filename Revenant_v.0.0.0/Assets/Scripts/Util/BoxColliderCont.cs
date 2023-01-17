using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


public class BoxColliderCont : MonoBehaviour
{
    [ReadOnly] public int TileWidth = 0;
    [ReadOnly] public int TileHeight = 0;
    

    [OnInspectorInit]
    private void InitCalculate()
    {
        MathUtil mathUtil = GameObject.Find("UtilPrefab").GetComponent<MathUtil>();
        var boxCol = GetComponent<BoxCollider2D>();
        TileWidth = (int)mathUtil.GetPreciseVal(boxCol.size.x / 0.32f);
        TileHeight = (int)mathUtil.GetPreciseVal(boxCol.size.y / 0.32f);
    }
    
    
    [Button]
    public void AddSize(Vector2 _addSize)
    {
        MathUtil mathUtil = GameObject.Find("UtilPrefab").GetComponent<MathUtil>();
        var boxCol = GetComponent<BoxCollider2D>();
        
        boxCol.size = new Vector2( boxCol.size.x + mathUtil.GetTileValue((int)_addSize.x), 
            boxCol.size.y + mathUtil.GetTileValue((int)_addSize.y));
        boxCol.offset = new Vector2( boxCol.size.x / 2f, boxCol.size.y / 2f);

        boxCol.size = mathUtil.GetPreciseVal(boxCol.size);
        boxCol.offset = mathUtil.GetPreciseVal(boxCol.offset);
        
        InitCalculate();
    }
    
    [Button]
    public void ChangeSize(Vector2 _changeSize)
    {
        MathUtil mathUtil = GameObject.Find("UtilPrefab").GetComponent<MathUtil>();
        var boxCol = GetComponent<BoxCollider2D>();
        
        boxCol.size = new Vector2( mathUtil.GetTileValue((int)_changeSize.x), 
            mathUtil.GetTileValue((int)_changeSize.y));
        boxCol.offset = new Vector2( boxCol.size.x / 2f, boxCol.size.y / 2f);

        boxCol.size = mathUtil.GetPreciseVal(boxCol.size);
        boxCol.offset = mathUtil.GetPreciseVal(boxCol.offset);
        
        InitCalculate();
    }
}
