using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBullet : MonoBehaviour
{
    public float Speed;
    public int Damage;
    public bool isTarget;
    public Transform target;

    void Update()
    {
        if (!isTarget)
        {
            transform.position += Vector3.forward * Time.deltaTime * Speed;

            if (transform.position.z > 200)
            {
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.PBullets, gameObject);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * Speed/10);

            if(target.gameObject.activeSelf==false)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.PBullets, gameObject);
        isTarget = false;
        target = null;
        GameObject ex = ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
        ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Die();
        }
        if (other.gameObject.tag == "SEnemy")
        {
            Die();
        }
    }
}
