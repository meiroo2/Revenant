using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    private bool isClicked = false;
    private float Timer = 0.1f;
    public int AimedObjid = -1;

    private void Update()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPosition;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        AimedObjid = collision.gameObject.GetInstanceID();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        AimedObjid = 1;
    }
}
