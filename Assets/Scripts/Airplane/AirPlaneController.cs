using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController : MonoBehaviour
{
    [Header("캐릭터 회전 정도")]
    [SerializeField] [Range(0, 10)] private float Horizontal_RotateDegree;
    [SerializeField] [Range(0, 10)] private float Vertical_RotateDegree;

    private float HorizontalInput;
    private float VerticalInput;

    void Start()
    {
        InitRocalRotation();
    }

    // 실행시 각도 0으로 초기화
    private void InitRocalRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        RotatePerFrame();
        StoppingEvent();
    }

    // 프레임마다 회전할 각도가 있다면 회전하기
    private void RotatePerFrame()
    {
        transform.Rotate(new Vector3(-VerticalInput * Vertical_RotateDegree, 0, -HorizontalInput * Horizontal_RotateDegree));
    }

    // 플레이어가 정지해있을 때 실행되는 이벤트
    private void StoppingEvent()
    {
        if (HorizontalInput == 0 && VerticalInput == 0)
        {
            StopConfirm();
        }
    }

    // 플레이어가 정지해있는지 확인하는 검사
    private void StopConfirm()
    {
        if (transform.rotation != Quaternion.identity)
        {
            ResetRotation();
            ReturnRotation();
        }
    }

    // 각도가 1 미만이면 0으로 초기화해주기
    private void ResetRotation()
    {
        if (Mathf.Abs(transform.rotation.x) < 1)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
        }
        if (Mathf.Abs(transform.localEulerAngles.y) < 1)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        if (Mathf.Abs(transform.localEulerAngles.z) < 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 0));
        }
    }

    // 각도가 1 이상이면 각도를 부드럽게 0으로 만들기
    private void ReturnRotation()
    {
        if (transform.rotation.x != 0)
        {
            transform.Rotate(new Vector3(transform.rotation.x > 0 ? -0.1f : 0.1f, 0, 0));
        }
        if (transform.rotation.y != 0)
        {
            transform.Rotate(new Vector3(transform.rotation.y > 0 ? -0.1f : 0.1f, 0, 0));
        }
        if (transform.rotation.z != 0)
        {
            transform.Rotate(new Vector3(transform.rotation.z > 0 ? -0.1f : 0.1f, 0, 0));
        }
    }

    void FixedUpdate()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        //transform.position += (new Vector3(h, v, 0));
    }
}
