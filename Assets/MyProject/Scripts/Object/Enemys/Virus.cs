using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Enemy
{
    private void OnEnable()
    {
        base.Setting();
    }

    void Update()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
