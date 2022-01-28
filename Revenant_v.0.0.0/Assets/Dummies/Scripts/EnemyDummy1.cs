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

        // y축 값이 -10보다 작으면 리스폰
        respawn();
    }

    private void FixedUpdate()
    {
        MoveYInput();
    }

    
}
