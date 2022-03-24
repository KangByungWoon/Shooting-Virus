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
            GameObject enemy = Instantiate(Enemy);
            enemy.transform.position = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10);

            yield return new WaitForSeconds(Random.Range(0.2f, 3f));
        }
    }
}