using System.Collections;
using UnityEngine;

public class EnemyFlag : MonoBehaviour
{
    public GameObject dummy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        dummy.GetComponent<EnemyDummy1>().DummyCall();
    }
}
