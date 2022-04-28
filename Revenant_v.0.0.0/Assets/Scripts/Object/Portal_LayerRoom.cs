using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_LayerRoom : MonoBehaviour
{
    public Portal_LayerRoom P_OtherSide;

    private CameraMove m_MainCam;

    private void Awake()
    {
        m_MainCam = Camera.main.GetComponent<CameraMove>();
    }

    public void moveObjToOtherSide(Transform _obj, bool _isPlayer)
    {
        if (_isPlayer)
        {
            float TempyVal = transform.position.y - _obj.position.y;
            Vector2 TempMoveVec = new Vector2(P_OtherSide.transform.position.x, P_OtherSide.transform.position.y - TempyVal);

            m_MainCam.InstantMoveToPlayer(_obj.position, TempMoveVec);
            _obj.position = TempMoveVec;
        }
        else
        {
            float TempyVal = transform.position.y - _obj.position.y;
            _obj.position = new Vector2(P_OtherSide.transform.position.x, P_OtherSide.transform.position.y - TempyVal);
        }
    }
}
