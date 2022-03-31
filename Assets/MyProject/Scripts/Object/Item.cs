using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] int HPP;
    [SerializeField] int GPM;
    private bool isDie = false;
    public enum ItemType
    {
        UpgradeWeapon,
        HideChar,
        HpUp,
        PainDown,
        AllKill,
        LevelUp
    }

    public ItemType Type;

    void Start()
    {
        int randomType = Random.Range(0, 6);

        switch (randomType)
        {
            case 0:
                Type = ItemType.UpgradeWeapon;
                break;
            case 1:
                Type = ItemType.HideChar;
                break;
            case 2:
                Type = ItemType.HpUp;
                break;
            case 3:
                Type = ItemType.PainDown;
                break;
            case 4:
                Type = ItemType.AllKill;
                break;
            case 5:
                Type = ItemType.LevelUp;
                break;
        }
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 5, 0) * Time.deltaTime * 10);

        transform.position += Vector3.back * Time.deltaTime * Speed;

        if (transform.position.z < -5)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EffectActive(GameObject effect)
    {
        GameObject effectobj = Instantiate(effect, GameManager.Instance.Player.transform);
        yield return new WaitForSeconds(2);
        Destroy(effectobj);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isDie)
        {
            isDie = true;
            GameManager.Instance.GetItem++;

            if (GameManager.Instance.Player.GetComponent<AirPlaneController>().WeaponLevel == 1)
            {
                Type = ItemType.UpgradeWeapon;
            }

            AirPlaneController player = GameManager.Instance.Player.GetComponent<AirPlaneController>();
            switch (Type)
            {
                case ItemType.UpgradeWeapon:
                    player.WeaponUpgrade();
                    StartCoroutine(EffectActive(player.WeaponUpEffect));
                    break;
                case ItemType.HideChar:
                    player.InvinActive(0, 2.5f, true);
                    StartCoroutine(EffectActive(player.InvinEffect));
                    break;
                case ItemType.HpUp:
                    GameManager.Instance.Hp += HPP;
                    StartCoroutine(EffectActive(player.HpUpEffect));
                    break;
                case ItemType.PainDown:
                    GameManager.Instance.Gp -= GPM;
                    StartCoroutine(EffectActive(player.PPDownEffect));
                    break;
                case ItemType.AllKill:
                    var enemys = FindObjectsOfType<Enemy>();
                    foreach (var enes in enemys)
                    {
                        if (!enes.isTarget && enes.EnemyType != ObjectPool.PoolType.RedBlood_Cells)
                        {
                            GameObject rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.PRockets, transform.position + new Vector3(Random.Range(-10, 10), 5, -10));
                            rocket.GetComponent<Rocket>().Target = enes.transform;

                            enes.isTarget = true;
                            enes.RocketObj = rocket;
                            enes.OnMark();

                            enes.TargetSetting();
                        }
                    }
                    break;
                case ItemType.LevelUp:
                    player._Level++;
                    break;
            }

            Destroy(gameObject);
        }
    }
}
