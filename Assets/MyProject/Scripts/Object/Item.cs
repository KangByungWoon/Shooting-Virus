using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] float Speed;
    public enum ItemType
    {
        UpgradeWeapon,
        HideChar,
        HpUp,
        PainDown,
        AllKill
    }

    public ItemType Type;

    void Start()
    {

    }

    void Update()
    {
        transform.position += Vector3.back * Time.deltaTime * Speed;

        if (transform.position.z < -5)
        {
            Destroy(gameObject);
        }
    }
}
