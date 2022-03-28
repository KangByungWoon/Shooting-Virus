using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController : MonoBehaviour
{
    [SerializeField] float Horizontal_RotPower;
    [SerializeField] float Vertical_RotPower;

    [SerializeField] RectTransform LockOn;

    [SerializeField] GameObject RocketPrefab;

    private IEnumerator InvinCorou;

    private float HorizontalInput = 0;
    private float VerticalInput = 0;

    public float xAngel = 0;
    public float yAngel = 0;
    public float zAngle = 0;

    private float xPos = 0;

    private float _yPos = 0;
    private float yPos
    {
        get { return _yPos; }
        set
        {
            if (value < -10)
            {
                _yPos = -10;
                xAngel = 0;
            }
            else if (value > 10)
            {
                _yPos = 10;
                xAngel = 0;
            }
            else
                _yPos = value;
        }
    }

    Vector3 StartPosition;
    [SerializeField] Vector3 TargetPoint;
    [SerializeField] MeshRenderer invinmat;

    public bool isInvin;

    void Start()
    {
        invinmat.material.mainTextureScale = new Vector2(0, 1);
        Setting();
    }

    private void Setting()
    {
        StartPosition = transform.position;
        TargetPoint = transform.position;
    }

    void Update()
    {
        transformRotate();
        Move();
        HorizontalEvent();
        VerticalEvent();
        LockOnSystem();
    }

    private void transformRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(xAngel, yAngel, zAngle), Time.deltaTime * 5);
    }
    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPoint, Time.deltaTime * 5);

    }
    private void HorizontalEvent()
    {
        if (HorizontalInput != 0)
        {
            yAngel = HorizontalInput * Horizontal_RotPower;
            zAngle = -HorizontalInput * Horizontal_RotPower;
            xPos += HorizontalInput * Time.deltaTime * 30;
            TargetPoint = StartPosition + new Vector3(xPos, yPos, 0);
        }
        else
        {
            zAngle = 0;
            yAngel = 0;
        }
    }
    private void VerticalEvent()
    {
        if (VerticalInput != 0)
        {
            xAngel = -VerticalInput * Vertical_RotPower;
            yPos += VerticalInput * 0.2f;
            TargetPoint = StartPosition + new Vector3(xPos, yPos, 0);
        }
        else
        {
            xAngel = 0;
        }
    }

    private void LockOnSystem()
    {
        for (float i = -3; i <= 3; i += 0.1f)
        {
            for (float j = -2; j <= 2; j += 0.1f)
            {
                Debug.DrawRay(transform.position + new Vector3(i, 0.5f + j, 0), transform.forward);
                if (Physics.Raycast(transform.position + new Vector3(i, 0.5f + j, 0), transform.forward, out var hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy")))
                {
                    if (!hit.transform.gameObject.GetComponent<Enemy>().isTarget)
                    {
                        GameObject rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.PRockets, transform.position + new Vector3(Random.Range(-10, 10), 5, -10));
                        rocket.GetComponent<Rocket>().Target = hit.transform.gameObject.transform;

                        Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                        enemy.isTarget = true;
                        enemy.RocketObj = rocket;
                        enemy.OnMark();

                        enemy.TargetSetting();
                    }
                }
            }
        }
    }

    public void InvinActive(int damage, float waitTime = 1f, bool isItem = false)
    {
        if (isInvin == false || isItem==true)
        {
            GameManager.Instance.Hp -= damage;
            isInvin = true;
            if(InvinCorou != null)
            {
                StopCoroutine(InvinCorou);
            }
            InvinCorou = InvinCoroutine(waitTime);
            StartCoroutine(InvinCorou);
        }
    }

    private IEnumerator InvinCoroutine(float waitTime)
    {
        invinmat.material.mainTextureScale = new Vector2(20, 1);

        for (int i = 0; i < 100; i++)
        {
            invinmat.material.mainTextureScale -= new Vector2(0.2f, 0);
            yield return new WaitForSeconds(waitTime / 100);
        }
        yield return new WaitForSeconds(0.5f);
        isInvin = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Item")
        {
            Debug.Log("IteGet");
        }
    }


    private void FixedUpdate()
    {
        GetAxis();
    }

    private void GetAxis()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
    }
}
