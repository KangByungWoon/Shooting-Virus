using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float Speed;
    void Start()
    {
        
    }

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
            GameObject ex = ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
            ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
            Camera.main.GetComponent<CameraSystem>().CameraShake(0.25f, 0.3f);
            Destroy(gameObject);
        }
    }
}
