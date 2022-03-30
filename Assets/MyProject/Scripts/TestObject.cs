using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    public Transform Target;

    void Start()
    {
    }

    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, Target.position, Time.deltaTime * 0.5f);
    }
}
