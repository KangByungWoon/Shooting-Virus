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
        LevelUp
    }

    public ItemType Type;

    // 아이템의 종류를 랜덤으로 정해줍니다.
    void Start()
    {
        int randomType = Random.Range(0, 5);

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

    // 아이템의 타입에 따라 이펙트와 효과 이벤트를 실행합니다.
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
                    GameManager.Instance.GetItemTxtOutput("WEAPON UPGRADE");
                    StartCoroutine(EffectActive(player.WeaponUpEffect));
                    break;
                case ItemType.HideChar:
                    player.InvinActive(0, 2.5f, true);
                    GameManager.Instance.GetItemTxtOutput("INVINCIBILITY");
                    StartCoroutine(EffectActive(player.InvinEffect));
                    break;
                case ItemType.HpUp:
                    GameManager.Instance.Hp += HPP;
                    GameManager.Instance.GetItemTxtOutput("HP UP");
                    StartCoroutine(EffectActive(player.HpUpEffect));
                    break;
                case ItemType.PainDown:
                    GameManager.Instance.Gp -= GPM;
                    GameManager.Instance.GetItemTxtOutput("PP DOWN");
                    StartCoroutine(EffectActive(player.PPDownEffect));
                    break;
                case ItemType.LevelUp:
                    player._Level++;
                    GameManager.Instance.GetItemTxtOutput("LEVEL UP");
                    break;
            }

            Destroy(gameObject);
        }
    }
}
