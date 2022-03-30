using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini : MonoBehaviour
{
    public int WeaponLevel = 1;
    [SerializeField] GameObject Player;
    [SerializeField] Vector3 Offset;

    private IEnumerator AttackCoroutine;
    public void StartFire()
    {
        AttackCoroutine = FireBullet();
        StartCoroutine(AttackCoroutine);
    }

    void Update()
    {
        transform.position = Player.transform.position + Offset;
        transform.rotation = Player.transform.rotation;
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            if (WeaponLevel == 1)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPool.Instance.PBullets, transform.position + new Vector3(0, 0.1f, 1));
                bullet.GetComponent<PBullet>().Speed = 40;
                bullet.GetComponent<PBullet>().Damage = 10;
            }
            else
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPool.Instance.Raises, transform.position + new Vector3(0, 0.1f, 1));
                bullet.GetComponent<PBullet>().Speed = 60;
                bullet.GetComponent<PBullet>().Damage = 20;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
