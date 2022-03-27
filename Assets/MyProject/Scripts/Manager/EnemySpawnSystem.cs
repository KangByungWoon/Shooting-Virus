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
            for (int i = 0; i < 2; i++)
            {
                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Bacterias, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                    case 1:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Germs, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                    case 2:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Viruses, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                    case 3:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Cancer_Cellses, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                }

            }
            yield return new WaitForSeconds(Random.Range(0.4f, 1.2f));
        }
    }
}
