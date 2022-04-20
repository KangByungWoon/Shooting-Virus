using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Enemy
{
    private void OnEnable()
    {
        base.Setting();
    }

    protected override void StartSetting()
    {
        base.StartSetting();
        Damage = json.Information.Virus_Damage / 2;
        Hp = json.Information.Virus_Hp;
        MoveSpeed = json.Information.Virus_Speed;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
