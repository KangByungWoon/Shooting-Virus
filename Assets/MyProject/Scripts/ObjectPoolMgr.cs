using System.Collections.Generic;
using UnityEngine;
using System;

using KeyType = System.String;

public class ObjectPoolMgr : Singleton<ObjectPoolMgr>
{
    // 풀링할 오브젝트의 데이터를 받아오는 List입니다.
    [SerializeField]
    private List<PoolObjectData> poolObjectDataList = new List<PoolObjectData>();

    // 풀링할 오브젝트의 샘플 오브젝트의 Dictionary입니다
    private Dictionary<KeyType, PoolObject> sampleDict;

    // 풀링할 오브젝트의 데이터 Dictionary입니다.
    private Dictionary<KeyType, PoolObjectData> dataDict;

    // 오브젝트풀들을 관리하는 Dictionary입니다
    private Dictionary<KeyType, Stack<PoolObject>> poolDict;

    private Dictionary<KeyType, GameObject> t_poolParent;

    private GameObject sampleTransform_Parent;

    private void Awake()
    {
        Init();
    }

    // 오브젝트풀에 넣을 오브젝트의 데이터를 통해 다른 Dictionary데이터를 할당해줍니다.
    private void Init()
    {
        int length = poolObjectDataList.Count;
        if (length == 0) return;

        sampleTransform_Parent = new GameObject();
        sampleTransform_Parent.name = "sampleTransform_Parent";
        sampleTransform_Parent.transform.parent = gameObject.transform;

        sampleDict = new Dictionary<KeyType, PoolObject>(length);
        dataDict = new Dictionary<KeyType, PoolObjectData>(length);
        poolDict = new Dictionary<KeyType, Stack<PoolObject>>(length);
        t_poolParent = new Dictionary<KeyType, GameObject>(length);

        foreach (var data in poolObjectDataList)
        {
            Register(data);
        }
    }

    // 각각의 오브젝트 풀을 관리하는 Dictionary에서 인자로 들어온 데이터의 키 값이 있는지 찾습니다.
    // Dictionary에 인자로 들어온 키 값이 없으면 데이터의 값을 참조해 샘플오브젝트를 만듭니다.
    // 오브젝트들을 관리할 Stack을 만들고 클론을 사용하여 오브젝트들을 생성 및 할당해줍니다.
    private void Register(PoolObjectData data)
    {
        if (poolDict.ContainsKey(data.key))
        {
            return;
        }

        GameObject sample = Instantiate(data.prefab, sampleTransform_Parent.transform);
        if (!sample.TryGetComponent(out PoolObject po))
        {
            po = sample.AddComponent<PoolObject>();
            po.key = data.key;
        }
        sample.SetActive(false);

        Stack<PoolObject> pool = new Stack<PoolObject>(data.MaxCreateCount);

        GameObject parent = new GameObject();
        parent.name = data.key;
        parent.transform.parent = gameObject.transform;

        for (int i = 0; i < data.InitCreateCount; i++)
        {
            PoolObject clone = po.Clone();
            clone.transform.parent = parent.transform;
            pool.Push(clone);
        }

        sampleDict.Add(data.key, po);
        dataDict.Add(data.key, data);
        poolDict.Add(data.key, pool);
        t_poolParent.Add(data.key, parent);
    }

    // 오브젝트 풀들을 관리하는 Dictionary에 인자로 들어온 키 값을 검색합니다.
    // 없으면 null을 반환하고 있다면 오브젝트를 활성화 하고 반환합니다.
    // 만약 오브젝트 풀 안에 오브젝트가 없다면 생성후 반환해줍니다.
    public PoolObject GetObject(KeyType key)
    {
        if (!poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        PoolObject po;

        if (pool.Count > 0)
        {
            po = pool.Pop();
        }
        else
        {
            po = sampleDict[key].Clone();
        }

        po.Activate();

        return po;
    }

    // 오브젝트 풀들을 관리하는 Dictionary에 인자로 들어온 키 값을 검색합니다.
    // 없으면 함수를 끝내고 있다면 찾은 Dictionary에 인자로 들어온 풀 오브젝트를 할당해주고 비활성화합니다.
    // 만약 Dictionary의 Count가 정했었던 최대 관리 가능 값 보다 크다면 Destroy로 해제해줍니다.
    public void ReleaseObject(PoolObject po)
    {
        if (!poolDict.TryGetValue(po.key, out var pool))
        {
            return;
        }

        KeyType key = po.key;

        if (pool.Count < dataDict[key].MaxCreateCount)
        {
            pool.Push(po);
            po.Deactivate();
        }
        else
        {
            Destroy(po.gameObject);
        }
    }
}
