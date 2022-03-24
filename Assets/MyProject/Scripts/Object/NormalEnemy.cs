using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PRocket")
        {
            base.Die();
            Debug.Log("스코어 추가");
        }
    }
}