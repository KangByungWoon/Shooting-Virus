using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leukocyte : Enemy
{
    [SerializeField] GameObject Item;
    private void OnEnable()
    {
        base.Setting();
    }

    void Update()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!isDie)
        {
            if (other.gameObject.tag == "PRocket")
            {
                if (other.GetComponent<Rocket>().Target == gameObject.transform)
                {
                    GameObject item = Instantiate(Item);
                    item.transform.position = transform.position;
                }
            }

            if (other.gameObject.tag == "Player" || other.gameObject.tag == "PBullet")
            {
                GameObject item = Instantiate(Item);
                item.transform.position = transform.position;
            }
        }
        base.OnTriggerEnter(other);
    }
}
