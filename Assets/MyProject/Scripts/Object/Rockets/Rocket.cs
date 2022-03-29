using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private float Speed;
    bool isDie = false;

    void OnEnable()
    {
        isDie = false;
    }

    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, Target.position, Time.deltaTime * Speed);
        transform.LookAt(Target.position);
        if (Target.gameObject.activeSelf == false)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isDie)
        {
            isDie = true;
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.PRockets, gameObject);
            GameObject ex = ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponentInParent<Enemy>().RocketObj == gameObject)
            {
                Die();
            }
        }
        if (other.gameObject.tag == "SEnemy")
        {
            if (other.gameObject.GetComponent<Enemy>().RocketObj == gameObject)
            {
                Die();
            }
        }
    }
}
