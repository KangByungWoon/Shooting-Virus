using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController : MonoBehaviour
{
    [Header("이동 가능 구간 지정")]
    [SerializeField] Vector2 XpositionRange;
    [SerializeField] Vector2 YpositionRange;

    [Header("캐릭터 회전 정도")]
    [SerializeField] [Range(0, 10)] private float Horizontal_RotateDegree;
    [SerializeField] [Range(0, 10)] private float Vertical_RotateDegree;

    [Header("캐릭터 이동 속도")]
    [SerializeField] [Range(0, 10)] private float Speed;

    private float HorizontalInput;
    private float VerticalInput;

    void Start()
    {
        InitRocalRotation();
    }

    // 실행시 각도 0으로 초기화
    private void InitRocalRotation()
    {
        transform.eulerAngles = Vector3.zero;
    }

    private void Update()
    {
        RotatePerFrame();
        SetRotation();
        //StoppingEvent();
    }

    // 프레임마다 회전할 각도가 있다면 회전하기
    private void RotatePerFrame()
    {
        transform.eulerAngles += new Vector3(-VerticalInput * Vertical_RotateDegree / 2, 0, -HorizontalInput * Horizontal_RotateDegree / 2);
    }

    private void SetRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles += new Vector3(0, 0, 1 * Horizontal_RotateDegree);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles += new Vector3(0, 0, -1 * Horizontal_RotateDegree);
        }
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
        if (transform.eulerAngles != Vector3.zero)
        {
            ResetRotation();
            ReturnRotation();
        }
    }

    // 각도가 1 미만이면 0으로 초기화해주기
    private void ResetRotation()
    {
        if (Mathf.Abs(transform.eulerAngles.x) < 1)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if (Mathf.Abs(transform.eulerAngles.y) < 1)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        if (Mathf.Abs(transform.eulerAngles.z) < 1)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    }

    // 각도가 1 이상이면 각도를 부드럽게 0으로 만들기
    private void ReturnRotation()
    {
        if (transform.eulerAngles.x != 0)
        {
            transform.eulerAngles += new Vector3(transform.rotation.x > 0 ? -0.1f : 0.1f, 0, 0);
        }
        if (transform.eulerAngles.y != 0)
        {
            transform.eulerAngles += new Vector3(transform.rotation.y > 0 ? -0.1f : 0.1f, 0, 0);
        }
        if (transform.eulerAngles.z != 0)
        {
            transform.eulerAngles += new Vector3(transform.rotation.z > 0 ? -0.1f : 0.1f, 0, 0);
        }
    }

    void FixedUpdate()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        Vector3 TargetPlusPosition = transform.position + new Vector3(HorizontalInput, VerticalInput, 0);
        if (TargetPlusPosition.x >= XpositionRange.x && TargetPlusPosition.x <= XpositionRange.y &&
            TargetPlusPosition.y >= YpositionRange.x && TargetPlusPosition.y <= YpositionRange.y)
        {
            transform.position += (new Vector3(HorizontalInput, 0, VerticalInput) * Speed);
        }
    }
}
