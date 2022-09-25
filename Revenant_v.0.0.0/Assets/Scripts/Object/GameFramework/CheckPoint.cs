using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _Renderer;

    public bool bCanInteract { get; set; }
    /** Indicate if the checkpoint is activated */
    public bool Activated = false;

    /** For animation */
    // private Animator thisAnimator;
    
    /** List with all checkpoints objects in the scene */
    public static List<GameObject> CheckPointsList;

    [Header("체크포인트 활성화 조건 버튼 - 맵 배치 적")] 
    public bool bEnemyListToActivate;
    public List<BasicEnemy> EnemyListToActivate;

    [Header("체크포인트 활성화 조건 버튼 - 스포너")] 
    public bool bEnemyListToActivateFromSpawner;
    public List<EnemySpawner> EnemyListToActivateFromSpawner;

    [Header("체크포인트 섹션 오브젝트")] 
    public List<GameObject> AssignedObjectsList;

    public int SectionNumber;
    
    private void Awake()
    {
        bCanInteract = false;

        _Renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //thisAnimator = GetComponent<Animator>();

        // Search all the checkpoints in the current scene
        CheckPointsList = GameObject.FindGameObjectsWithTag("CheckPoint").ToList();
    }

    private void Update()
    {
        
    }

    /** Get position of the last activated checkpoint */
    public static Vector2 GetActiveCheckPointPosition()
    {
        Debug.Log("GetActivateCheckPointPoistion");
        
        // If player die without activate any checkpoint, will return a default position
        Vector2 result = new Vector2(0, 0);

        if (CheckPointsList != null)
        {
            foreach (GameObject CheckPointGameObject in CheckPointsList)
            {
                // Search the activated checkpoint to get its position
                if (CheckPointGameObject.GetComponent<CheckPoint>().Activated)
                {
                    result = CheckPointGameObject.transform.position;
                    break;
                }
            }
        }
        
        return result;
    }
    
    /** Activate the checkpoint */
    public void ActivateCheckPoint()
    {
        // Deactive all checkpoints in the scene
        foreach (GameObject CheckPointGameObject in CheckPointsList)
        {
            CheckPointGameObject.GetComponent<CheckPoint>().Activated = false;
            //cp.GetComponent<Animator>().SetBool("Active", false);
        }

        // Activated the current checkpoint
        Activated = true;
        //thisAnimator.SetBool("Active", true);
    }

    public void SetActiveObjects()
    {
        int index = 0;
        foreach (GameObject CheckPointGameObject in CheckPointsList)
        {
            if (Activated == true && CheckPointGameObject.Equals(CheckPointsList[index]))
            {
                SectionNumber = index;
                
                Debug.Log(SectionNumber);

                for (int i = 0; i < AssignedObjectsList.Count; i++)
                {
                    AssignedObjectsList[i].SetActive(true);
                    Debug.Log(AssignedObjectsList[i].gameObject.activeSelf);
                }
                
                index++;
            }

        }

    }
    
    public void ActivateBothOutline(bool _isOn)
    {
        // Set bCanInteract When Enter/Exit the Collider
        bCanInteract = _isOn;
    }
}