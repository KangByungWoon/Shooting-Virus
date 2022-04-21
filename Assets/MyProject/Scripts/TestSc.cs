using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSc : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ObjectPoolMgr.Instance.GetObject("Bacteria", Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            var obj = FindObjectOfType<PoolObject>();
            ObjectPoolMgr.Instance.ReleaseObject(obj);
        }
    }
}
