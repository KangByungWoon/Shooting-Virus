using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERocket : MonoBehaviour
{
    public Transform Target;
    public Vector3 TargetPosition;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] protected float Speed;
    private bool isComplete = false;

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
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.ERockets, gameObject);
            }
        }
        catch (MissingReferenceException)
        {
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.ERockets, gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.ERockets, gameObject);
            GameObject ex = ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
            Camera.main.GetComponent<CameraSystem>().CameraShake(0.25f, 0.3f);
        }
    }
}
