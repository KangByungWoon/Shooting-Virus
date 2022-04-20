using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancer_Cells : Enemy
{
    private void OnEnable()
    {
        base.Setting();
    }

    protected override void StartSetting()
    {
        base.StartSetting();
        Damage = json.Information.Cancer_Cells_Damage / 2;
        Hp = json.Information.Cancer_Cells_Hp;
        MoveSpeed = json.Information.Cancer_Cells_Speed;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
