using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    void Start()
    {
        ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.TestQueue, gameObject, 2f);
    }

    void Update()
    {
        
    }
}
