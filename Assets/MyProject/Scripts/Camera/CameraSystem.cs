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

    // 플레이어를 따라서 움직이고 각도를 회전해줍니다. 플레이어보다 조금 늦게 움직여서 따라가는데 텀을 주었습니다.
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(Player.xAngel, Player.yAngel, Player.zAngle), Time.deltaTime * MoveSpeed);

        transform.position = Vector3.Lerp(transform.position, Player.transform.position + Offset + ShakePosition, Time.deltaTime * MoveSpeed * 3);
    }

    // 카메라 쉐이크 기능입니다. 카메라를 정해준 시간, 파워만큼 흔듭니다.
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
