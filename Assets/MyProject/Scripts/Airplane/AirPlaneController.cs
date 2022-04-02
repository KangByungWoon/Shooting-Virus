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

    private void transformRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.Euler(xAngel, yAngel, zAngle), Time.deltaTime * MoveSpeed);
    }
    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPoint, Time.deltaTime * MoveSpeed);

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

    IEnumerator FireBullet(bool isTarget, bool Raise = false)
    {
        while (true)
        {
            if (!NOSHOTTING)
            {
                GameObject bullet = null;
                if (!Raise)
                {
                    bullet = ObjectPool.Instance.GetObject(ObjectPool.Instance.PBullets, transform.position + new Vector3(0, 0.1f, 1));
                    bullet.GetComponent<PBullet>().Speed = BulletMoveSpeed;
                    bullet.GetComponent<PBullet>().Damage = BulletDamage;
                }
                else
                {
                    bullet = ObjectPool.Instance.GetObject(ObjectPool.Instance.Raises, transform.position + new Vector3(0, 0.1f, 1));
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
                                bullet.GetComponent<PBullet>().Speed = BulletMoveSpeed + 5;
                                bullet.GetComponent<PBullet>().Damage = BulletDamage + 5;
                                bullet.GetComponent<PBullet>().isRaise = true;
                            }
                            bullet.GetComponent<PBullet>().target = hit.transform.gameObject.transform;
                            bullet.GetComponent<PBullet>().isTarget = true;
                        }
                    }
                }
                else
                {
                    bullet.GetComponent<PBullet>().isTarget = false;
                }
            }
            yield return new WaitForSeconds(BulletAttackSpeed);
        }
    }

    private void LockOnSystem()
    {
        if (!NOSHOTTING)
        {
            var hitObjs = Physics.BoxCastAll(transform.position + new Vector3(0, 0.5f, 0), new Vector3(AttackPlace, AttackPlace, AttackPlace), transform.forward, transform.rotation, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var hit in hitObjs)
            {
                if (!hit.transform.gameObject.GetComponent<Enemy>().isTarget && hit.transform.gameObject.GetComponent<Enemy>().EnemyType != ObjectPool.PoolType.RedBlood_Cells)
                {
                    GameObject rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.PRockets, transform.position + new Vector3(Random.Range(-10, 10), 5, -10));
                    rocket.GetComponent<Rocket>().Target = hit.transform.gameObject.transform;
                    rocket.GetComponent<Rocket>().NoTarget = false;

                    Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                    enemy.isTarget = true;
                    enemy.RocketObj = rocket;
                    enemy.OnMark();

                    enemy.TargetSetting();
                }
            }
        }
    }
    private IEnumerator RocketFire()
    {
        while (true)
        {
            if (!NOSHOTTING)
            {
                GameObject rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.PRockets, transform.position + new Vector3(0, 0.1f, 2f));
                rocket.GetComponent<Rocket>().NoTarget = true;
                rocket.GetComponent<Rocket>().Damage = BulletDamage * 10;

            }
            yield return new WaitForSeconds(BulletAttackSpeed * 2);
        }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        float x = Random.Range(0f, 1024012401f);
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
