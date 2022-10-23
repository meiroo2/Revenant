using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStartTrigger : MonoBehaviour
{
    [field: SerializeField] public DialogSequence p_DialogSequence;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!p_DialogSequence.isDialogStart && Input.GetKeyDown(KeyCode.F))
        {
            p_DialogSequence.isDialogStart = true;
        }
    }
}
