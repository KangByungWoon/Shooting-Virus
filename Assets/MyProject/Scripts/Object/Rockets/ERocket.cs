using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERocket : MonoBehaviour
{
    public Transform Target;
    public Vector3 TargetPosition;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] public float Speed;
    private bool isComplete = false;
    public ObjectPool.PoolType rocketType;
    public int Damage;
    bool isAttack = false;

    private void OnEnable()
    {
        isComplete = false;
        isAttack = false;
    }

    void Update()
    {
        try
        {
            if (!isComplete)
            {
                TargetPosition = Target.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime * Speed);
            transform.LookAt(TargetPosition);
            if (transform.position.z <= TargetPosition.z + 5f && !isComplete)
            {
                TargetPosition += transform.forward * 5;
                isComplete = true;
            }
            else if (transform.position == TargetPosition && isComplete)
            {
                RocketRelease();
            }
        }
        catch (MissingReferenceException)
        {
            RocketRelease();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isAttack)
        {
            isAttack = true;
            RocketRelease();
            other.GetComponentInParent<AirPlaneController>().InvinActive(Damage);
            GameObject ex = ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
            Camera.main.GetComponent<CameraSystem>().CameraShake(0.25f, 0.3f);
        }

        if(other.tag=="Enemy" && other.gameObject.transform == Target)
        {
            isAttack = true;
            RocketRelease();
            GameObject ex = ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
        }
    }

    private void RocketRelease()
    {
        switch (rocketType)
        {
            case ObjectPool.PoolType.Bacteria:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.BacteriaRockets, gameObject);
                break;
            case ObjectPool.PoolType.Germ:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.GermRockets, gameObject);
                break;
            case ObjectPool.PoolType.Virus:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.VirusRockets, gameObject);
                break;
            case ObjectPool.PoolType.Cancer_Cells:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Cancer_CellsRockets, gameObject);
                break;
        }
    }
}
