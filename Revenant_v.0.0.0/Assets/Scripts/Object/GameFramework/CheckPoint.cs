using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Sirenix.OdinInspector;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _Renderer;

    public bool bCanInteract { get; set; }

    /** Indicate if the checkpoint is activated */
    [ReadOnly] public bool bActivated = false;

    /** For animation */
    // private Animator thisAnimator;

    /** List with all checkpoints objects in the scene */
    public static List<CheckPoint> CheckPointsList;

    private GameObject test;
    private CheckPoint Test_2;
    
    [Header("체크포인트 활성화 조건 버튼 - 맵 배치 적")] public bool bEnemyListToActivate;
    public List<BasicEnemy> EnemyListToActivate;

    [Header("체크포인트 활성화 조건 버튼 - 스포너")] public bool bEnemyListToActivateFromSpawner;
    public List<EnemySpawner> EnemyListToActivateFromSpawner;
    public List<GameObject> EnemyListFromSpawner;

    [Header("체크포인트 섹션 오브젝트")] public List<GameObject> AssignedObjectsList;

    public int SectionNumber;

    private void Awake()
    {
        bCanInteract = false;

        _Renderer = GetComponent<SpriteRenderer>();
        SetUpEnemyList();
    }

    void Start()
    {
        //thisAnimator = GetComponent<Animator>();

        // 현재 씬에 있는 모든 체크포인트를 찾기 (이름 순으로 정렬)
        CheckPointsList = FindObjectsOfType<CheckPoint>().OrderBy(CP => CP.name).ToList();

        SetUpSectionNumber();
        SetUpIsActivated();
        DestroyAssignedObjects();
    }

    /** 적 리스트 초기화 함수 */
    void SetUpEnemyList()
    {
        // 스포너 적 리스트 초기화
        for (int i = 0; i < EnemyListToActivateFromSpawner.Count; i++)
        {
            EnemyListFromSpawner = new List<GameObject>(EnemyListToActivateFromSpawner[i].p_WillSpawnEnemys);
        }
    }
    
    /** 섹션 번호 초기화 함수 */
    void SetUpSectionNumber()
    {
        // 각 체크포인트에 섹션 번호 부여
        for (int i = 0; i < CheckPointsList.Count; i++)
        {
            CheckPointsList[i].SectionNumber = i + 1;
        }
    }
    
    void SetUpIsActivated()
    {
        // 저장된 데이터 중 체크포인트가 활성화 되어있다면 체크포인트의 섹션을 판별
        if (DataHandleManager.Instance.IsCheckPointActivated && DataHandleManager.Instance.CheckPointSectionNumber == SectionNumber)
        {
            // 해당 체크포인트를 활성화
            CheckPointsList[SectionNumber - 1].bActivated = DataHandleManager.Instance.IsCheckPointActivated;
        }
    }

    /** 할당된 오브젝트들을 파괴하는 함수 */
    void DestroyAssignedObjects()
    {
        // 체크포인트가 활성화 되어있고 섹션 번호가 해당되는 체크포인트와 같거나 작은 체크포인트들을 확인 후
        if (bActivated && SectionNumber <= CheckPointsList[SectionNumber - 1].SectionNumber)
        {
            for (int i = 0; i < SectionNumber; i++)
            {
                // 스포너 내부 적 제거
                DestroyAssignedObjectsList(CheckPointsList[i].EnemyListFromSpawner);
                // 등록된 오브젝트 제거 (스포너 등록 가능)
                DestroyAssignedObjectsList(CheckPointsList[i].AssignedObjectsList);
            }
        }
    }

    /** 등록된 오브젝트들 찾아서 제거 후 리스트 비우는 함수 */
    void DestroyAssignedObjectsList(List<GameObject> TargetObjectsList)
    {
        // 오브젝트들을 전부 검색 후
        foreach (GameObject ObjectLists in TargetObjectsList)
        {
            // 오브젝트 제거
            Destroy(ObjectLists);
        }
        // 리스트 비우기
        TargetObjectsList.Clear();
    }

    /** 마지막으로 활성화된 체크포인트를 가져오는 함수 */
    public static Vector2 GetActiveCheckPointPosition()
    {
        // 플레이어가 체크포인트를 활성화 하지 않고 죽으면 기본 포지션 반환
        Vector2 result = new Vector2(0, 0);

        // 체크포인트 리스트가 존재하면
        if (CheckPointsList != null)
        {
            // 모든 체크포인트 찾은 뒤
            foreach (CheckPoint CheckPointGameObject in CheckPointsList)
            {
                // 활성화된 체크포인트를 찾아서
                if (CheckPointGameObject.bActivated)
                {
                    // 체크포인트 위치를 결과값에 반환
                    result = CheckPointGameObject.transform.position;
                    break;
                }
            }
        }

        return result;
    }

    /** 체크포인트 활성화 함수 */
    public void ActivateCheckPoint()
    {
        // 씬에 있는 모든 체크포인트를 비활성화
        foreach (CheckPoint CheckPointGameObject in CheckPointsList)
        {
            CheckPointGameObject.bActivated = false;
            //cp.GetComponent<Animator>().SetBool("Active", false);
        }
        // 현재 체크포인트 활성화
        bActivated = true;
        // 체크포인트 활성화 여부 저장
        DataHandleManager.Instance.IsCheckPointActivated = bActivated;
        //thisAnimator.SetBool("Active", true);
    }

    public void ActivateBothOutline(bool _isOn)
    {
        // Collider 안에 (체크포인트 범위 안에) 플레이어가 들어오면 bCanInteract를 Set
        bCanInteract = _isOn;
    }
}