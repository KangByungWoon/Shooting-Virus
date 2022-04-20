using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlood_Cells : Enemy
{
    public int GPDamage;
    private void OnEnable()
    {
        base.Setting();
    }

    protected override void StartSetting()
    {
        base.StartSetting();
        MoveSpeed = json.Information.RedBlood_Cells_Speed;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!isDie)
        {
            if (other.gameObject.tag == "PRocket")
            {
                if (other.GetComponent<Rocket>().Target == gameObject.transform)
                {
                    GameManager.Instance.Gp += GPDamage;
                }
            }
            if(other.gameObject.tag == "ERocket")
            {
                if(other.GetComponent<ERocket>().Target == gameObject.transform)
                {
                    GameManager.Instance.Gp += GPDamage;
                }
            }

            if (other.gameObject.tag == "Player" || other.gameObject.tag == "PBullet")
            {
                GameManager.Instance.Gp += GPDamage;
            }
        }
        base.OnTriggerEnter(other);
    }
}
