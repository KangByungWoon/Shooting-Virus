using System.Collections.Generic;
using UnityEngine;
using System;

using KeyType = System.String;

public class ObjectPoolMgr : Singleton<ObjectPoolMgr>
{
    [SerializeField]
    private List<PoolObjectData> poolObjectDataList = new List<PoolObjectData>();

    private Dictionary<KeyType, PoolObject> sampleDict;

    private Dictionary<KeyType, PoolObjectData> dataDict;

    private Dictionary<KeyType, Stack<PoolObject>> poolDict;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        int length = poolObjectDataList.Count;
        if (length == 0) return;

        sampleDict = new Dictionary<KeyType, PoolObject>(length);
        dataDict = new Dictionary<KeyType, PoolObjectData>(length);
        poolDict = new Dictionary<KeyType, Stack<PoolObject>>(length);

        foreach (var data in poolObjectDataList)
        {
            Register(data);
        }
    }

    private void Register(PoolObjectData data)
    {
        if (poolDict.ContainsKey(data.key))
        {
            return;
        }

        GameObject sample = Instantiate(data.prefab);
        if (!sample.TryGetComponent(out PoolObject po))
        {
            po = sample.AddComponent<PoolObject>();
            po.key = data.key;
        }
        sample.SetActive(false);

        Stack<PoolObject> pool = new Stack<PoolObject>(data.MaxCreateCount);

        for (int i = 0; i < data.InitCreateCount; i++)
        {
            PoolObject clone = po.Clone();
            pool.Push(clone);
        }

        sampleDict.Add(data.key, po);
        dataDict.Add(data.key, data);
        poolDict.Add(data.key, pool);
    }

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

    public void ReleaseObject(PoolObject po)
    {
        if(!poolDict.TryGetValue(po.key, out var pool))
        {
            return;
        }

        KeyType key = po.key;

        if(pool.Count < dataDict[key].MaxCreateCount)
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
