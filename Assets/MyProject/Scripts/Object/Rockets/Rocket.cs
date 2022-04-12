using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private float Speed;
    bool isDie = false;
    public bool NoTarget = false;
    public int Damage;

    void OnEnable()
    {
        isDie = false;
        NoTarget = false;
    }

    // 유도 미사일이라면 할당된 타겟을 향해서 유도이동을하며, 그렇지 않다면 앞으로 직진이동을합니다.
    void Update()
    {
        if (NoTarget)
        {
            transform.position += Vector3.forward * Time.deltaTime * Speed * 7;

            if (transform.position.z > 200)
            {
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.PRockets, gameObject);
            }
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, Target.position, Time.deltaTime * Speed);
            transform.LookAt(Target.position);
            if (Target.gameObject.activeSelf == false)
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
        if (other.gameObject.tag == "Boss")
        {
            Die();
        }
    }
}
