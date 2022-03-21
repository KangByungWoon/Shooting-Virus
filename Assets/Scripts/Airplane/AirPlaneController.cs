using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController : MonoBehaviour
{
    [Header("이동 가능 구간 지정")]
    [SerializeField] Vector2 XpositionRange;
    [SerializeField] Vector2 ZpositionRange;

    [Header("캐릭터 회전 정도")]
    [SerializeField][Range(0, 10)] private float Horizontal_RotateDegree;
    [SerializeField][Range(0, 10)] private float Vertical_RotateDegree;

    [Header("캐릭터 이동 속도")]
    [SerializeField][Range(0, 10)] private float Speed;

    private float HorizontalInput;
    private float VerticalInput;
    private float zangle = 0;
    private float yangle = 0;

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
        Character_Move();
        Character_RotatePerFrame();
        transform.position = new Vector3(transform.position.x, 10, transform.position.z);
    }

    //private void Character_Move()
    //{
    //    if(Input.GetKeyDown(KeyCode.LeftShift))
    //    {
    //        Speed *= 2;
    //    }
    //    if(Input.GetKeyUp(KeyCode.LeftShift))
    //    {
    //        Speed /= 2;
    //    }

    //    Vector3 TargetPlusPosition = transform.position + new Vector3(HorizontalInput, 0, VerticalInput);
    //    if (TargetPlusPosition.x >= XpositionRange.x && TargetPlusPosition.x <= XpositionRange.y)
    //    {
    //        transform.position += (new Vector3(HorizontalInput, 0, 0) * Speed);
    //    }
    //    if (TargetPlusPosition.z >= ZpositionRange.x && TargetPlusPosition.z <= ZpositionRange.y)
    //    {
    //        transform.position += (new Vector3(0, 0, VerticalInput) * Speed);
    //    }
    //}

    private void Character_Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Speed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Speed /= 2;
        }

        //Vector3 TargetPlusPosition = transform.position + new Vector3(HorizontalInput, 0, VerticalInput);
        //if (TargetPlusPosition.x >= XpositionRange.x && TargetPlusPosition.x <= XpositionRange.y)
        //{
        //    transform.position += (new Vector3(HorizontalInput, 0, 0) * Speed);
        //}
        transform.position += transform.forward * 1 * Speed;
    }

    // 프레임마다 회전할 각도가 있다면 회전하기
    //private void Character_RotatePerFrame()
    //{
    //    transform.rotation = Quaternion.Slerp(transform.rotation,
    //    Quaternion.Euler(0, 0, Mathf.Clamp(angle, -30f, 30f)), Time.deltaTime * Horizontal_RotateDegree);
    //    if (HorizontalInput != 0)
    //    {
    //        Debug.Log(-Horizontal_RotateDegree * HorizontalInput);
    //        angle += -Horizontal_RotateDegree * HorizontalInput;
    //    }
    //    else
    //    {
    //        angle = 0;
    //    }
    //}

    private void Character_RotatePerFrame()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(0, Mathf.Clamp(-yangle, -180f, 180f), Mathf.Clamp(zangle, -30f, 30f)), Time.deltaTime * Horizontal_RotateDegree);
        if (HorizontalInput != 0)
        {
            Debug.Log(-Horizontal_RotateDegree * HorizontalInput);
            zangle += -Horizontal_RotateDegree * HorizontalInput;
            yangle += -Horizontal_RotateDegree * HorizontalInput;
        }
        else
        {
            zangle = 0;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            HorizontalInput = 0;
        }
        else
        {
            HorizontalInput = Input.GetAxis("Horizontal");
        }
        VerticalInput = Input.GetAxis("Vertical");
    }
}