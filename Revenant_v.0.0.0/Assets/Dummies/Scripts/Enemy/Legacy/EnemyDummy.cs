using System.Collections;
using UnityEngine;


// 상속 스크립트 확인용
public class EnemyDummy : Human
{
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidBody;

    // 현대 문법 적용
    public int h { get; set; }

    public void MoveToX(int n) { transform.position = new Vector3(n, 0); }

    public float getYPos() { return transform.position.y; }

    public void ChangeColor(Color _input) { spriteRenderer.color = _input; }

    public virtual void DummyCall()
    {
        rigidBody.velocity += new Vector2(1, 0);
        Debug.Log("Parent Dummy");
    }

    public IEnumerator MoveXTo(int n)
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector2(n, transform.position.y);
    }

    public void MoveYInput()
    {
        //if(Input.GetKeyDown(KeyCode.UpArrow))
        rigidBody.velocity = new Vector2(0, Input.GetAxisRaw("Vertical"));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter");
    }

}
