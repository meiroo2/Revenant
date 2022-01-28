using System.Collections;
using UnityEngine;

public class EnemyDummy1 : EnemyDummy
{
    // Start is called before the first frame update
    void Start()
    {
        h = 3;
        //Debug.Log(3);
        MoveToX(h);
        ChangeColor(Color.cyan);
    }

    private void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Debug.Log(getYPos());
        StartCoroutine(MoveXTo(3));
    }

    private void FixedUpdate()
    {
        MoveYInput();
    }

    //IEnumerator MoveXTo(int n)
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    transform.position = new Vector2(n, transform.position.y);
    //}
}
