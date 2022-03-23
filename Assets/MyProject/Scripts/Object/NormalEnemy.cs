using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        base.Die();
        Destroy(other.gameObject);
        Debug.Log("스코어 추가");
    }
}