using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_TargetParts : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer srenderer;

    float m_power = 100;
    [SerializeField]
    float m_remainTime = 3.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        srenderer = GetComponent<SpriteRenderer>();
        Spread();
    }

    // spread target Parts
    public void Spread()
    {
        rigid.AddForce((new Vector2(Random.Range(-1f, 1f), Random.Range(0, 1f)))* m_power);
        Invoke(nameof(Delete), m_remainTime);
    }

    public void Delete()
    {
        StartCoroutine(FadeOut());   
    }

    public IEnumerator FadeOut()
    {
        for (float f = 1f; f > 0; f -= 0.02f)
        {
            srenderer.color = new Color(1, 1, 1, f);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        
    }
}