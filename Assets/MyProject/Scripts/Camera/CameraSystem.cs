using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private AirPlaneController Player;
    [SerializeField] private Vector3 Offset;

    void Start()
    {

    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(Player.xAngel, Player.yAngel, Player.zAngle), Time.deltaTime * 5);

        transform.position = Vector3.Lerp(transform.position, Player.transform.position + Offset, Time.deltaTime * 15);
    }
}
