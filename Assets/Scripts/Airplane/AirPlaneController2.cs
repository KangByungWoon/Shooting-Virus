using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController2 : MonoBehaviour
{
    [SerializeField] float Horizontal_RotPower;
    [SerializeField] float Vertical_RotPower;

    private float HorizontalInput;
    private float VerticalInput;

    private float xAngel;
    private float zAngle;

    private float xPos;

    Vector3 StartPosition;
    Vector3 TargetPoint;

    [SerializeField] GameObject main_camera;

    void Start()
    {
        StartPosition = transform.position;
        TargetPoint = transform.position;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(Mathf.Clamp(xAngel, -15, 15), 0, Mathf.Clamp(zAngle, -15f, 15f)), Time.deltaTime * 5);

        transform.position = Vector3.Lerp(transform.position, TargetPoint, Time.deltaTime * 5);
        //main_camera.transform.position = Vector3.Lerp(main_camera.transform.position, main_camera.transform.position +
            //new Vector3(xPos,0,0), Time.deltaTime);

        if (HorizontalInput != 0)
        {
            zAngle += -HorizontalInput * Horizontal_RotPower;
            xPos += HorizontalInput * Time.deltaTime * 5;
            TargetPoint = StartPosition + new Vector3(xPos, 0, 0);
        }
        else
        {
            zAngle = 0;
        }

        if (VerticalInput != 0)
        {
            xAngel += -VerticalInput * Vertical_RotPower;
            TargetPoint = StartPosition + new Vector3(xPos, VerticalInput, 0);
        }
        else
        {
            xAngel = 0;
        }
    }

    private void FixedUpdate()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
    }
}
