using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TargetBoard_HeadHotBox : MonoBehaviour
{
    TargetBoard targetBoard;
    public void HitHotBox(IHotBoxParam _param)
    {
        targetBoard.hitParts = PARTS.HEAD;
        targetBoard.Attacked();
    }
}
