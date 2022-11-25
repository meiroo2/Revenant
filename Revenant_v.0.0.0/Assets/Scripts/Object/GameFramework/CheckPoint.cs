using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool bCanInteract { get; set; }

    /** Indicate if the checkpoint is activated */
    [ReadOnly] public bool bActivated = false;

    /** For animation */
    public Animator p_Animator;

    /** List with all checkpoints objects in the scene */
    public static List<CheckPoint> CheckPointsList;

    private GameObject test;
    private CheckPoint Test_2;

    [Header("체크포인트 활성화 조건 버튼 - 맵 배치 적")] public bool bEnemyListToActivate;
    public List<BasicEnemy> EnemyListToActivate;

    [Header("체크포인트 활성화 조건 버튼 - 스포너")] public bool bEnemyListToActivateFromSpawner;
    public List<EnemySpawner> EnemyListToActivateFromSpawner;
    [ReadOnly] public List<GameObject> EnemyListFromSpawner;

    [Header("체크포인트 섹션 오브젝트")] public List<GameObject> AssignedObjectsList;

    public int SectionNumber;
    
    private Coroutine m_AnimCheckCoroutine;
    private readonly int Activate = Animator.StringToHash("Activate");
    private readonly int AfterIdle = Animator.StringToHash("AfterIdle");

    private void Awake()
    {
        bCanInteract = false;

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
        
        if (bActivated)
        {
            p_Animator.SetInteger(Activate, 1);
            p_Animator.SetInteger(AfterIdle, 1);
        }
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
        DataHandleManager dataMgr = GameMgr.GetInstance().p_DataHandleMgr;
        if (dataMgr.IsCheckPointActivated &&
            dataMgr.CheckPointSectionNumber == SectionNumber)
        {
            // 해당 체크포인트를 활성화
            CheckPointsList[SectionNumber - 1].bActivated = dataMgr.IsCheckPointActivated;
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
            //ObjectLists.SetActive(false);
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
                    Vector2 returnPos;
                    int m_LayerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("EmptyFloor"));

                    returnPos = Physics2D.Raycast(CheckPointGameObject.transform.position,
                        -CheckPointGameObject.transform.up, 1f, m_LayerMask).point;
                    returnPos.y += 0.64f;

                    result = returnPos;
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
        }

        // 현재 체크포인트 활성화
        bActivated = true;
        // 체크포인트 활성화 여부 저장
        DataHandleManager dataMgr = GameMgr.GetInstance().p_DataHandleMgr;
        dataMgr.IsCheckPointActivated = bActivated;
        

        // 애니메이션
        p_Animator.enabled = true;
        if (!ReferenceEquals(m_AnimCheckCoroutine, null))
        {
            StopCoroutine(m_AnimCheckCoroutine);
            m_AnimCheckCoroutine = null;
        }
        p_Animator.SetInteger(Activate, 1);
        m_AnimCheckCoroutine = StartCoroutine(CheckSaveAnimEnd());
    }

    /// <summary>
    /// Check Activate Animation EndTime.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckSaveAnimEnd()
    {
        while (true)
        {
            yield return null;
            if (p_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                p_Animator.SetInteger(AfterIdle, 1);
                break;
            }
        }

        yield break;
    }

    public void ActivateBothOutline(bool _isOn)
    {
        // Collider 안에 (체크포인트 범위 안에) 플레이어가 들어오면 bCanInteract를 Set
        bCanInteract = _isOn;
    }
}