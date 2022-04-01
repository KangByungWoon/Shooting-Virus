using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private AirPlaneController Player;
    [SerializeField] private Vector3 Offset;
    private Vector3 ShakePosition;

    private float MoveSpeed;
    private IEnumerator cameraShake_Coroutine;

    void Start()
    {
        MoveSpeed = JsonSystem.Instance.Information.PlayerMoveSpeed;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(Player.xAngel, Player.yAngel, Player.zAngle), Time.deltaTime * MoveSpeed);

        transform.position = Vector3.Lerp(transform.position, Player.transform.position + Offset + ShakePosition, Time.deltaTime * MoveSpeed * 3);
    }

    public void CameraShake(float duration, float ShakePower = 1)
    {
        if (cameraShake_Coroutine != null)
        {
            StopCoroutine(cameraShake_Coroutine);
        }

        StartCoroutine(cameraShake(duration, ShakePower));
    }

    IEnumerator cameraShake(float duration, float ShakePower = 1)
    {
        for (float i = 0; i < duration; i += 0.01f)
        {
            ShakePosition = Random.insideUnitSphere * ShakePower;
            yield return new WaitForSeconds(0.01f);
        }
        ShakePosition = Vector3.zero;
    }
}
