using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    O_Tree, O_Rock,
    I_SpeedUp, I_PowerUp, I_Recovery
}

[Serializable]  // 인스펙터 에서 편집 가능하도록 설정
public class PoolObjectDataDict
{
    public PoolObjectType type;
    public int initCount; // 초기에 생성할 개수
    public GameObject original; // 생성할 오브젝트의 원본
}

public class PoolManager_Dict : MonoBehaviour
{
    public static PoolManager_Dict instance; // 싱글톤 인스턴스

    [SerializeField]    // PoolObjectType 순서와 무조건 일치하게 파싱하기!!!
    public PoolObjectDataDict[] poolObjDatas; // = new PoolObjectData[(int)PoolObjectType.Length]; // 풀 오브젝트 데이터 배열

    /// <summary>
    /// 풀링 딕셔너리
    /// </summary>
    Dictionary<PoolObjectType, Queue<GameObject>> objectDict;

    /// <summary>
    /// 부모를 쉽게 찾기 위한 부모 배열
    /// </summary>
    Transform[] parents;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        objectDict = new Dictionary<PoolObjectType, Queue<GameObject>>();

        CreateParents();    // 부모 생성
        InitPoolSet(); // 풀 초기화
    }

    /// <summary>
    /// 풀 초기화 함수
    /// </summary>
    void InitPoolSet()
    {
        foreach (var data in poolObjDatas)
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            // 데이터 타입 키에 새로운 큐 할당
            objectDict[data.type] = newQueue;

            for (int i = 0; i < data.initCount; i++)
            {
                newQueue.Enqueue(CreateObject(data));
            }
        }
    }

    /// <summary>
    /// 풀매니저의 자식으로 각 풀링 오브젝트들의 부모 오브젝트를 만들어주는 함수
    /// </summary>
    void CreateParents()
    {
        parents = new Transform[poolObjDatas.Length];

        for (int i = 0; i < poolObjDatas.Length; i++)
        {
            // 새로운 오브젝트를 만들고 데이터 타입에 맞게 이름 설정
            GameObject parent = new GameObject(poolObjDatas[i].type.ToString());
            // 만든 오브젝트를 자신(PM)의 자식으로 설정
            parent.transform.SetParent(transform);

            // 부모 배열에 만든 오브젝트 넣기
            parents[i] = parent.transform;
        }
    }

    /// <summary>
    /// 오브젝트 풀에 부족할 때 추가 생성
    /// </summary>
    /// <param name="index">생성해줄 오브젝트의 데이터</param>
    /// <returns>생성한 오브젝트</returns>
    GameObject CreateObject(PoolObjectDataDict data)
    {
        int createIndex = (int)data.type;

        // 데이터의 원본을 복제하고 부모를 세팅
        GameObject obj = Instantiate(data.original, parents[createIndex]);
        // 이름 설정
        obj.name = data.type.ToString();

        return obj; // 생성된 오브젝트 반환
    }

    /// <summary>
    /// 풀에서 오브젝트를 가져오는 함수
    /// </summary>
    /// <param name="type">가져올 타입</param>
    /// <returns>가져온 오브젝트</returns>
    public GameObject GetPool(PoolObjectType type)
    {
        // 키값이 있는지 -> 딕셔너리가 초기화 되고 각 키값에 맞는 큐를 넣어줬는지
        if (!objectDict.ContainsKey(type))
        {
            Debug.LogWarning($"PoolManager : {type}에 대한 풀 정보가 없습니다");
            return null;
        }

        // 삽입할 큐 설정
        var queue = objectDict[type];
        GameObject returnObj;
       
        if (queue.Count == 0) // 큐가 비어있으면 추가 생성
        {
            var data = poolObjDatas[(int)type];
            returnObj = CreateObject(data);
        }
        else
        {
            returnObj = queue.Dequeue();
        }

        returnObj.SetActive(true);
        return returnObj;
    }

    public void SetPool(GameObject setObj, PoolObjectType type)
    {
        if (!objectDict.ContainsKey(type))
        {
            Debug.LogWarning($"PoolManager: {type}에 대한 풀 정보가 없습니다.");
            return; 
        }

        setObj.SetActive(false); // 비활성화
        objectDict[type].Enqueue(setObj); // 큐에 추가
    }

    public void ClearPool()
    {
        // 이건 딕셔너리를 아예 비우는 거라
        // objectDict.Clear();

        // 풀 오브젝트 부모들의 자식들만 전부 꺼주기
        foreach (var parent in parents)
        {
            foreach (Transform child in parent)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
