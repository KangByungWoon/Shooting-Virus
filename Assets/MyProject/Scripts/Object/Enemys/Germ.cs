using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Germ : Enemy
{
    private void OnEnable()
    {
        base.Setting();
    }

    protected override void StartSetting()
    {
        base.StartSetting();
        Damage = json.Information.Germ_Damage / 2;
        Hp = json.Information.Germ_Hp;
        MoveSpeed = json.Information.Germ_Speed;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
