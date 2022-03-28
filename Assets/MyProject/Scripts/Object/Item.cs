using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] int HPP;
    [SerializeField] int GPM;
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

        switch(randomType)
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
        transform.Rotate(new Vector3(0,5,0) * Time.deltaTime * 10);

        transform.position += Vector3.back * Time.deltaTime * Speed;

        if (transform.position.z < -5)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);

            switch (Type)
            {
                case ItemType.UpgradeWeapon:
                    other.GetComponent<AirPlaneController>().WeaponUpgrade();
                    break;
                case ItemType.HideChar:
                    GameManager.Instance.Player.GetComponent<AirPlaneController>().InvinActive(0, 2.5f, true);
                    break;
                case ItemType.HpUp:
                    GameManager.Instance.Hp += HPP;
                    break;
                case ItemType.PainDown:
                    GameManager.Instance.Gp -= GPM;
                    break;
                case ItemType.AllKill:
                    var enemys = FindObjectsOfType<Enemy>();
                    foreach(var enes in enemys)
                    {
                        if(!enes.isTarget && enes.EnemyType != ObjectPool.PoolType.RedBlood_Cells)
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
                    GameManager.Instance.Player.GetComponent<AirPlaneController>()._Level++;
                    break;
            }
        }
    }
}
