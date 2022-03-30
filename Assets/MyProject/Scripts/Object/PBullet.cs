using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBullet : MonoBehaviour
{
    public float Speed;
    public int Damage;
    public bool isTarget;
    public Transform target;
    public bool isRaise = false;
    public bool isDie = false;

    private void OnEnable()
    {
        isDie = false;
    }

    void Update()
    {
        if (!isTarget)
        {
            transform.position += Vector3.forward * Time.deltaTime * Speed * 7;

            if (transform.position.z > 200)
            {
                if (isRaise)
                {
                    ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Raises, gameObject);
                }
                else
                {
                    ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.PBullets, gameObject);
                }
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * Speed / 20);

            if (target.gameObject.activeSelf == false)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (!isDie)
        {
            isDie = true;
            if (isRaise)
            {
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Raises, gameObject);
            }
            else
            {
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.PBullets, gameObject);
            }

            isTarget = false;
            target = null;
            GameObject ex = ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "SEnemy" || other.gameObject.tag =="Boss")
        {
            Die();
        }
    }
}
