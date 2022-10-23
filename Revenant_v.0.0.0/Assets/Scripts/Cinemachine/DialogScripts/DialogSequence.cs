using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSequence : MonoBehaviour
{
    public bool isDialogStart = false;
    public int DialogCount = 0;
    private DialogBox currentBox;
	[field: SerializeField] public GameObject Hologram;

	// Update is called once per frame
	void Update()
    {
        if (!isDialogStart)
            return;

        if(DialogCount < transform.childCount)
        { 
            if(DialogCount == 0)
            {
				transform.GetChild(0).gameObject.SetActive(true);  
				currentBox = transform.GetChild(0).GetComponent<DialogBox>();
                Hologram.SetActive(true);
			}

			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
            {
                currentBox.SkipEvent?.Invoke();
                if (currentBox.isTextEnd)
                {
                    currentBox.gameObject.SetActive(false);
                    DialogCount++;
                    if(DialogCount < transform.childCount)
                    {
                        transform.GetChild(DialogCount).gameObject.SetActive(true);
					    currentBox = transform.GetChild(DialogCount).GetComponent<DialogBox>();
                    }
                    else
                    {
						Hologram.SetActive(false);
					}
				}
			}
        }
    }
}
