using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private float Speed;

    void Start()
    {
    }

    void Update()
    {
        try
        {
            transform.position = Vector3.Slerp(transform.position, Target.position, Time.deltaTime * Speed);
            transform.LookAt(Target.position);
        }
        catch (MissingReferenceException)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Target.gameObject.tag)
        {
            Destroy(gameObject);
        }
    }
}
