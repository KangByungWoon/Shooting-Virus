using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirPlaneController : MonoBehaviour
{
    [SerializeField] float Horizontal_RotPower;
    [SerializeField] float Vertical_RotPower;

    [SerializeField] GameObject Mini1;
    [SerializeField] GameObject Mini2;

    [SerializeField] Text LvText;
    [SerializeField] Text ExpText;
    [SerializeField] Image LvBar;

    [SerializeField] RectTransform LockOn;
    [SerializeField] MeshRenderer invinmat;

    Vector3 TargetPoint;

    [HideInInspector] public bool isInvin;

    float BulletAttackSpeed = 0.5f;
    float BulletMoveSpeed;
    int BulletDamage;
    [SerializeField] int AttackPlace;

    public GameObject LevelUpEffect;
    public GameObject ShiledEffect;
    public GameObject HpUpEffect;
    public GameObject PPDownEffect;
    public GameObject WeaponUpEffect;
    public GameObject InvinEffect;

    public bool NOSHOTTING = false;

    public int Level;
    public int _Level
    {
        get { return Level; }
        set
        {
            if (Level != MaxLevel)
            {
                Level++;
                BulletDamage += 1;
                BulletMoveSpeed += 1;
                BulletAttackSpeed -= 0.015f;

                if (Level == 3)
                {
                    Mini1.SetActive(true);
                    Mini1.gameObject.GetComponent<Mini>().StartFire();
                }
                else if (Level == 6)
                {
                    Mini2.SetActive(true);
                    Mini2.gameObject.GetComponent<Mini>().StartFire();
                }
                else if (Level == 10)
                {
                    Mini1.gameObject.GetComponent<Mini>().WeaponLevel = 2;
                    Mini2.gameObject.GetComponent<Mini>().WeaponLevel = 2;
                }

                LvText.text = "LV." + Level.ToString();
            }
            else
            {
                LvText.text = "LV.MAX";
            }

        }
    }
    public int MaxLevel;
    public float Exp;
    public float _Exp
    {
        get { return Exp; }
        set
        {
            if (value >= MaxExp)
            {
                Exp = 0;
                MaxExp += 100;
                _Level++;
                GameManager.Instance.GetItemTxtOutput("", true);
                Destroy(GameObject.Instantiate(LevelUpEffect, gameObject.transform), 2f);
            }
            else if (value < MaxExp && Level < MaxLevel)
            {
                Exp = value;
            }
            LvBar.fillAmount = Exp / MaxExp;
            ExpText.text = Mathf.Round((Exp / MaxExp) * 100).ToString() + "%";
        }
    }
    public float MaxExp;

    private IEnumerator InvinCorou;
    private IEnumerator AttackCorou;

    private Vector3 StartPosition;

    [HideInInspector] public float MoveSpeed;
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

    public int WeaponLevel = 1;

    void Start()
    {
        invinmat.material.mainTextureScale = new Vector2(0, 1);
        Setting();
        AttackCorou = FireBullet(false);
        StartCoroutine(AttackCorou);
    }

    // 변수들에 미리 할당해야하는 값을 할당 해줍니다.
    private void Setting()
    {
        StartPosition = transform.position;
        TargetPoint = transform.position;

        MoveSpeed = JsonSystem.Instance.Information.PlayerMoveSpeed;
        BulletAttackSpeed = JsonSystem.Instance.Information.PlayerBulletAttackSpeed;
        BulletMoveSpeed = JsonSystem.Instance.Information.PlayerBulletMoveSpeed;
        BulletDamage = JsonSystem.Instance.Information.PlayerDamage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WeaponUpgrade();
        }

        transformRotate();
        Move();
        HorizontalEvent();
        VerticalEvent();
        if (WeaponLevel == 5)
        {
            LockOnSystem();
        }
    }

    // 플레이어가 목표 각도로 부드럽게 회전합니다.
    private void transformRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(xAngel, yAngel, zAngle), Time.deltaTime * MoveSpeed);
    }

    // 플레이어가 목표 지점으로 부드럽게 이동합니다.
    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPoint, Time.deltaTime * MoveSpeed);

    }
    
    // 좌우 방향키를 눌렀을 때 받는 이벤트입니다. x축을 증감시키고 각도를 지정합니다.
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

    // 상하 방향키를 눌렀을 때 받는 이벤트입니다. y축을 증감시키고 각도를 지정합니다.
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

    // 코루틴으로 총알을 발사합니다. 총알의 단계에 따라 지정해주고 유도탄이라면 레이케스트를 발사하여 오브젝트를 판별합니다.
    IEnumerator FireBullet(bool isTarget, bool Raise = false)
    {
        while (true)
        {
            if (!NOSHOTTING)
            {
                PBullet bullet = null;
                if (!Raise)
                {
                    bullet = ObjectPool.Instance.GetObject(ObjectPool.Instance.PBullets, transform.position + new Vector3(0, 0.1f, 1)).GetComponent<PBullet>();
                    bullet.Speed = BulletMoveSpeed;
                    bullet.Damage = BulletDamage;
                }
                else
                {
                    bullet = ObjectPool.Instance.GetObject(ObjectPool.Instance.Raises, transform.position + new Vector3(0, 0.1f, 1)).GetComponent<PBullet>();
                }
                if (isTarget)
                {
                    var hitObjs = Physics.BoxCastAll(transform.position + new Vector3(0, 0.5f, 0), new Vector3(AttackPlace, AttackPlace, AttackPlace), transform.forward, transform.rotation, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy"));
                    foreach (var hit in hitObjs)
                    {
                        if (hit.transform.gameObject.GetComponent<Enemy>().EnemyType != ObjectPool.PoolType.RedBlood_Cells)
                        {
                            if (Raise)
                            {
                                bullet.Speed = BulletMoveSpeed + 5;
                                bullet.Damage = BulletDamage + 5;
                                bullet.isRaise = true;
                            }
                            bullet.target = hit.transform.gameObject.transform;
                            bullet.isTarget = true;
                        }
                    }
                }
                else
                {
                    bullet.isTarget = false;
                }
            }
            yield return new WaitForSeconds(BulletAttackSpeed);
        }
    }

    // 미사일 락온 시스템입니다. 특정 사거리 안에 박스 레이케스트를 발사하고 Enemy Layer을 가지고 있는 오브젝트가 판별된다면 지정하고 발사합니다.
    private void LockOnSystem()
    {
        if (!NOSHOTTING)
        {
            var hitObjs = Physics.BoxCastAll(transform.position + new Vector3(0, 0.5f, 0), new Vector3(AttackPlace, AttackPlace, AttackPlace), transform.forward, transform.rotation, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var hit in hitObjs)
            {
                if (!hit.transform.gameObject.GetComponent<Enemy>().isTarget && hit.transform.gameObject.GetComponent<Enemy>().EnemyType != ObjectPool.PoolType.RedBlood_Cells)
                {
                    Rocket rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.PRockets, transform.position + new Vector3(Random.Range(-10, 10), 5, -10)).GetComponent<Rocket>();
                    rocket.Target = hit.transform.gameObject.transform;
                    rocket.NoTarget = false;

                    Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                    enemy.isTarget = true;
                    enemy.RocketObj = rocket.gameObject;
                    enemy.OnMark();

                    enemy.TargetSetting();
                }
            }
        }
    }

    // 락온 없이 발사되는 미사일입니다. 할당되고 앞으로 나아갑니다.
    private IEnumerator RocketFire()
    {
        while (true)
        {
            if (!NOSHOTTING)
            {
                Rocket rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.PRockets, transform.position + new Vector3(0, 0.1f, 2f)).GetComponent<Rocket>();
                rocket.NoTarget = true;
                rocket.Damage = BulletDamage * 10;

            }
            yield return new WaitForSeconds(BulletAttackSpeed * 2);
        }
    }

    // 무적 함수입니다. 무적 이펙트 코루틴을 실행시킵니다.
    public void InvinActive(int damage, float waitTime = 1f, bool isItem = false)
    {
        if (isInvin == false || isItem == true)
        {
            GameManager.Instance.Hp -= damage;
            isInvin = true;
            if (InvinCorou != null)
            {
                StopCoroutine(InvinCorou);
            }
            InvinCorou = InvinCoroutine(waitTime);
            StartCoroutine(InvinCorou);
        }
    }

    // 무적 이펙트 코루틴 입니다. 메테리얼로 이펙트를 구현했습니다.
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

    public void WeaponUpgrade()
    {
        if (WeaponLevel < 5)
        {
            WeaponLevel++;
            switch (WeaponLevel)
            {
                case 2:
                    StopCoroutine(AttackCorou);
                    AttackCorou = FireBullet(true);
                    StartCoroutine(AttackCorou);
                    break;
                case 3:
                    AttackPlace += 3;
                    LockOn.localScale += new Vector3(3, 3, 0);
                    break;
                case 4:
                    StopCoroutine(AttackCorou);
                    AttackCorou = FireBullet(true, true);
                    StartCoroutine(AttackCorou);
                    break;
                case 5:
                    if (AttackCorou != null)
                        StopCoroutine(AttackCorou);
                    StartCoroutine(RocketFire());
                    break;
            }
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
