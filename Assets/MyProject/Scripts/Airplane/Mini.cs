using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 옆에서 작게 생성되어 총알을 쏴주는 오브젝트입니다. 기본 기능은 플레이어의 무기와 유사합니다.
public class Mini : MonoBehaviour
{
    public int WeaponLevel = 1;
    [SerializeField] AirPlaneController Player;
    [SerializeField] Vector3 Offset;

    private IEnumerator AttackCoroutine;

    void Update()
    {
        Move();
        Rotation();
    }

    private void Move()
    {
        transform.position = Player.transform.position + Offset;
    }

    private void Rotation()
    {
        transform.rotation = Player.transform.rotation;
    }

    public void StartFire()
    {
        AttackCoroutine = FireBullet();
        StartCoroutine(AttackCoroutine);
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (Player.NOSHOTTING)
                continue;

            if (WeaponLevel == 1)
            {
                PBullet bullet = ObjectPoolMgr.Instance.GetObject("PBullet", transform.position + new Vector3(0, 0.1f, 1)).GetComponent<PBullet>();
                bullet.Speed = 40;
                bullet.Damage = 10;
            }
            else
            {
                PBullet bullet = ObjectPoolMgr.Instance.GetObject("PRaise", transform.position + new Vector3(0, 0.1f, 1)).GetComponent<PBullet>();
                bullet.Speed = 60;
                bullet.Damage = 20;
            }
        }
    }
}
