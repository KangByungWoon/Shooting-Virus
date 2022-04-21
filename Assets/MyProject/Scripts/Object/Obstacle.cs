using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float Speed;

    // 앞으로 직진이동합니다.
    void Update()
    {
        transform.position += Vector3.back * Time.deltaTime * Speed;

        if(transform.position.z < -3)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponentInParent<AirPlaneController>().InvinActive(10);
            PoolObject ex = ObjectPoolMgr.Instance.GetObject("Particle", gameObject.transform.position);
            ObjectPoolMgr.Instance.ReleaseObject(ex, 2f);
            Camera.main.GetComponent<CameraSystem>().CameraShake(0.25f, 0.3f);
            Destroy(gameObject);
        }
    }
}
