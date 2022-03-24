using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public override void Start()
    {
        base.Start();
        GameObject rocket = Instantiate(ERocket);
        rocket.transform.position = transform.position;
        rocket.GetComponent<ERocket>().Target = GameManager.Instance.Player.transform.position;
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