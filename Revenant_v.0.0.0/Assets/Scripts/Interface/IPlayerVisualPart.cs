using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerVisualPart
{
    public void SetVisible(bool _isVisible);
    public void SetAniVisible(bool _isVisible);
    public void SetAnim(string _ParamName, int _value);
    public void SetSprite(int _inputIdx);
}