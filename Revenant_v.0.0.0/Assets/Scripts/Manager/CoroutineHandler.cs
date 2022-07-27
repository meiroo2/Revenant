using System.Collections;
using UnityEngine;


public class CoroutineHandler : MonoBehaviour
{
    private IEnumerator enumerator = null;

    private void Coroutine(IEnumerator coro)
    {
        enumerator = coro;
        StartCoroutine(coro);        
    }

    private void Update()
    {
        if (enumerator is { Current: null })
        {
            Destroy(gameObject);
        }        
    }

    public void Stop()
    {
        StopCoroutine(enumerator.ToString());
        Destroy(gameObject);
    }

    public static CoroutineHandler Start_Coroutine(IEnumerator coro)
    {
        //Debug.Log("Coroutine Handler");
        
        var obj = new GameObject("CoroutineHandler");
        var handler = obj.AddComponent<CoroutineHandler>();
        if (handler)
        {
            handler.Coroutine(coro);
        }
        return handler;
    }
}