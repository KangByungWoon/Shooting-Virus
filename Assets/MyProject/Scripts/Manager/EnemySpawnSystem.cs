using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    public GameObject Enemy;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    void Update()
    {

    }

    IEnumerator Spawn()
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                 ObjectPool.Instance.GetObject(ObjectPool.Instance.Bacterias, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
            }
            yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
        }
    }
}
