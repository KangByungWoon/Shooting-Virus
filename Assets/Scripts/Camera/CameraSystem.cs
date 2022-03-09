using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Vector3 Offset;

    void Start()
    {

    }

    void Update()
    {
        transform.position = Player.transform.position + Offset;
    }
}
