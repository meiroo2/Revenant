using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStairObject : TutorialObject
{
    private Collider2D[] m_Colliders;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_Colliders = GetComponentsInChildren<Collider2D>();
		foreach (var collider in m_Colliders)
		{
			collider.enabled = false;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public override void Initialize()
	{
		action += NextAnimation;
	}

    public void ActiveCollider()
    {
		foreach (var collider in m_Colliders)
		{
			collider.enabled = true;
		}
	}
}
