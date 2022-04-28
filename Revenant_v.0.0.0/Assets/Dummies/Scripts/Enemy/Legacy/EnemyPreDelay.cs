using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPreDelay : MonoBehaviour
{
    public SpriteOutline[] m_Objs;
    public GameObject m_Player;

    private bool doPreDelay = false;
    private bool doReverse = false;
    private float m_outSize = 0.0f;

    private bool isEnd = false;

    public void playPreDelay()
    {
        if (!doPreDelay && !isEnd)
        {
            doPreDelay = true;
            foreach (SpriteOutline element in m_Objs)
            {
                element.outlineSize = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (doPreDelay)
        {
            if (!doReverse)
            {
                m_outSize += Time.fixedDeltaTime * 80f;
                if (m_outSize > 15f)
                    doReverse = true;
            }
            else
            {
                m_outSize -= Time.fixedDeltaTime * 80f;
                if (m_outSize < 0.1f)
                {
                    m_outSize = 0.0f;
                    doReverse = false;
                    doPreDelay = false;
                    isEnd = true;
                }
            }
        }
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, m_Player.transform.position) < 1f)
        {
            playPreDelay();
        }
        else if (Vector2.Distance(transform.position, m_Player.transform.position) > 1f && isEnd)
        {
            isEnd = false;
        }

        foreach (SpriteOutline element in m_Objs)
        {
           // element.outlineSize = m_outSize;
        }
    }
}
