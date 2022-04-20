using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : Enemy
{
    private void OnEnable()
    {
        base.Setting();
    }

    protected override void StartSetting()
    {
        base.StartSetting();
        Damage = json.Information.Bacteria_Damage / 2;
        Hp = json.Information.Bacteria_Hp;
        MoveSpeed = json.Information.Bacteria_Speed;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}