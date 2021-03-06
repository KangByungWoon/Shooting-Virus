using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CovidBoss : MonoBehaviour
{
    bool isDeath = false;
    public float Hp;
    public float _Hp
    {
        get { return Hp; }
        set
        {
            if (!isDeath)
            {
                if (value <= 0)
                {
                    if (_HpGague > 1)
                    {
                        Hp = 100;
                    }
                    else
                    {
                        Hp = 0;
                    }
                    _HpGague--;
                }
                else
                {
                    Hp = value;
                }
                HpText.text = ((Hp / 100) * 100).ToString() + "%";
                HpGagueImage.fillAmount = Hp / 100;
            }
        }
    }
    public int HpGague;
    public int _HpGague
    {
        get { return HpGague; }
        set
        {
            if (value <= 0)
            {
                isDeath = true;
                State = null;
                StopAllCoroutines();
                StartCoroutine(Death());
            }
            HpGague = value;
            HpGagueText.text = "X " + HpGague.ToString();
        }
    }

    [SerializeField] float RotSpeed;
    float MoveSpeed;
    float BulletSpeed;

    [SerializeField] Vector3 MinPos;
    [SerializeField] Vector3 MaxPos;

    [SerializeField] GameObject Effect;
    [SerializeField] GameObject Obstacle;

    [SerializeField] Text HpText;
    [SerializeField] Text HpGagueText;
    [SerializeField] Image HpGagueImage;

    [SerializeField] GameObject DeathEffect;

    BossState State = new BossState();

    public enum BossType
    {
        Covid,
        CovidChange
    }

    public BossType bossType;

    // 패턴을 시작하는 초기 셋팅입니다. 연출이 끝나면 불립니다.
    public void StartPattern()
    {
        State.Setting(this);
        State.Process();
        _Hp = 100;
        JsonSystem json = JsonSystem.Instance;
        if (bossType == BossType.Covid)
        {
            _HpGague = json.Information.Stage1Boss_HP;
            MoveSpeed = json.Information.Stage1Boss_Speed;
            BulletSpeed = json.Information.Stage1Boss_BulletSpeed;
        }
        else
        {
            _HpGague = json.Information.Stage2Boss_HP;
            MoveSpeed = json.Information.Stage2Boss_Speed;
            BulletSpeed = json.Information.Stage2Boss_BulletSpeed;
        }
    }

    void Update()
    {
        gameObject.transform.Rotate(new Vector3(1, 1, 1) * Time.deltaTime * RotSpeed);
    }

    // 이동 패턴입니다. 랜덤 지점을 정해서 움직입니다.
    public IEnumerator MovePattern()
    {
        Vector3 RandomPos = RandomVector3(MinPos, MaxPos);
        while (true)
        {
            transform.position = Vector3.Slerp(transform.position, RandomPos, Time.deltaTime * MoveSpeed);

            if ((transform.position - RandomPos).sqrMagnitude <= 0.1f)
            {
                transform.position = RandomPos;
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }

    }

    // HP에 따라 패턴을 선택합니다.
    public IEnumerator SelectAttackPattern()
    {
        int randomPattern = 0;
        if (HpGague > 40)
        {
            randomPattern = Random.Range(0, 2);
        }
        else if (HpGague <= 40 && HpGague > 30)
        {
            randomPattern = Random.Range(0, 2);
        }
        else if (HpGague <= 30 && HpGague > 20)
        {
            randomPattern = Random.Range(0, 4);
        }
        else if (HpGague <= 20 && HpGague > 10)
        {
            randomPattern = Random.Range(0, 6);
        }
        else if (HpGague <= 10 && HpGague > 0)
        {
            randomPattern = Random.Range(0, 7);
        }

        // 지정된 패턴을 실행합니다.
        IEnumerator WaitAttackCoroutine = null;
        switch (randomPattern)
        {
            case 0:
                WaitAttackCoroutine = SpawnEnemy(10);
                break;
            case 1:
                WaitAttackCoroutine = Normal_Shooting();
                break;
            case 2:
                WaitAttackCoroutine = RandomObtcle();
                break;
            case 3:
                WaitAttackCoroutine = UpAndSpawn();
                break;
            case 4:
                WaitAttackCoroutine = BurstFire();
                break;
            case 5:
                WaitAttackCoroutine = MathObstaclePattern();
                break;
            case 6:
                WaitAttackCoroutine = SpawnEnemy(10);
                break;
        }

        Instantiate(Effect, gameObject.transform);
        yield return StartCoroutine(WaitAttackCoroutine);
    }

    // 적을 소환하는 패턴입니다.
    public IEnumerator SpawnEnemy(int maxValue)
    {
        int randomspawn = Random.Range(2, maxValue);
        for (int i = 0; i <= randomspawn; i++)
        {
            int random = Random.Range(0, 4);
            Vector3 RandomPos = new Vector3(transform.position.x + Random.Range(-5f, 6f), transform.position.y + Random.Range(-5f, 6f), transform.position.z + Random.Range(-5f, 6f));
            switch (random)
            {
                case 0:
                    ObjectPoolMgr.Instance.GetObject("Bacteria", RandomPos);
                    break;
                case 1:
                    ObjectPoolMgr.Instance.GetObject("Germ", RandomPos);
                    break;
                case 2:
                    ObjectPoolMgr.Instance.GetObject("Cancer_Cells", RandomPos);
                    break;
                case 3:
                    ObjectPoolMgr.Instance.GetObject("Virus", RandomPos);
                    break;
            }
        }

        yield return new WaitForSeconds(3f);
    }

    public IEnumerator Normal_Shooting()
    {
        RocketShot();
        yield return new WaitForSeconds(2f);
    }

    // 지정된 위치에서 로켓을 생성합니다.
    private void RocketShot()
    {
        if (HpGague <= 40)
        {
            ReturnRandomRocket(transform.position + new Vector3(10, 10, 15));
            ReturnRandomRocket(transform.position + new Vector3(10, -10, 15));
            ReturnRandomRocket(transform.position + new Vector3(-10, -10, 15));
            ReturnRandomRocket(transform.position + new Vector3(-10, 10, 15));

            ReturnRandomRocket(transform.position + new Vector3(0, 15, 15));
            ReturnRandomRocket(transform.position + new Vector3(15, 0, 15));
            ReturnRandomRocket(transform.position + new Vector3(0, -15, 15));
            ReturnRandomRocket(transform.position + new Vector3(-15, 0, 15));
        }
        else
        {
            ReturnRandomRocket(transform.position + new Vector3(0, 10, 15));
            ReturnRandomRocket(transform.position + new Vector3(10, 0, 15));
            ReturnRandomRocket(transform.position + new Vector3(0, -10, 15));
            ReturnRandomRocket(transform.position + new Vector3(-10, 0, 15));
        }
    }

    // 랜덤 종류의 로켓을 지정해서 반환합니다.
    private void ReturnRandomRocket(Vector3 position)
    {
        int random = Random.Range(0, 4);
        ERocket rocket = null;
        switch (random)
        {
            case 0:
                rocket = ObjectPoolMgr.Instance.GetObject("BacteriaRocket", position).GetComponent<ERocket>();
                break;
            case 1:
                rocket = ObjectPoolMgr.Instance.GetObject("GermRocket", position).GetComponent<ERocket>();
                break;
            case 2:
                rocket = ObjectPoolMgr.Instance.GetObject("Cancer_CellsRocket", position).GetComponent<ERocket>();
                break;
            case 3:
                rocket = ObjectPoolMgr.Instance.GetObject("VirusRocket", position).GetComponent<ERocket>();
                break;
        }
        rocket.Target = GameManager.Instance.Player.transform;
        rocket.Speed *= 3;
    }

    // 장애물 소환 패턴입니다.
    public IEnumerator RandomObtcle()
    {
        IEnumerator ObstacleCorou = ObstacleCreate();
        StartCoroutine(ObstacleCorou);
        for (int i = 0; i < 5; i++)
        {
            IEnumerator MoveCorou = MovePattern();
            yield return StartCoroutine(MoveCorou);
        }

        StopCoroutine(ObstacleCorou);
    }

    // 지속적으로 증가하는 angle 변수에 따라 삼각함수로 이동을 합니다.
    private IEnumerator ObstacleCreate()
    {
        float angle = 0;
        while (true)
        {
            GameObject ob = Instantiate(Obstacle);
            ob.transform.position = transform.position + new Vector3(Mathf.Cos(angle) * Random.Range(0f, 3f), Mathf.Sin(angle) * Random.Range(0f, 3f), -3);
            angle += 0.5f;
            yield return new WaitForSeconds(0.02f);
        }
    }

    // 보스가 위로 올라가서 적을 스폰하는 패턴입니다.
    public IEnumerator UpAndSpawn()
    {
        Vector3 targetPos = new Vector3(transform.position.x, 50, transform.position.z);

        while (true)
        {
            transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * MoveSpeed);

            if ((transform.position - targetPos).sqrMagnitude <= 0.1f)
            {
                transform.position = targetPos;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        int randomspawn = Random.Range(10, 20);
        for (int i = 0; i <= randomspawn; i++)
        {
            int random = Random.Range(0, 4);
            Vector3 RandomPos = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10);
            switch (random)
            {
                case 0:
                    ObjectPoolMgr.Instance.GetObject("Bacteria", RandomPos);
                    break;
                case 1:
                    ObjectPoolMgr.Instance.GetObject("Germ", RandomPos);
                    break;
                case 2:
                    ObjectPoolMgr.Instance.GetObject("Cancer_Cells", RandomPos);
                    break;
                case 3:
                    ObjectPoolMgr.Instance.GetObject("Virus", RandomPos);
                    break;
            }
        }

        yield return new WaitForSeconds(10f);
    }

    // 보스가 미사일을 점사하는 패턴입니다. 코루틴으로 기다리며 미사일을 여러번 발사합니다.
    public IEnumerator BurstFire()
    {
        StartCoroutine(BurstFireCorou(new Vector3(0, 10, 0)));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(BurstFireCorou(new Vector3(0, -10, 0)));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(BurstFireCorou(new Vector3(10, 0, 0)));
        yield return new WaitForSeconds(0.2f);
        IEnumerator BurstCoroutine = BurstFireCorou(new Vector3(-10, 0, 0));
        yield return StartCoroutine(BurstCoroutine);
    }

    public IEnumerator BurstFireCorou(Vector3 pos)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                ReturnRandomRocket(transform.position + new Vector3(pos.x + j, pos.y + 0, pos.z + 15));
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator MathObstaclePattern()
    {
        IEnumerator ObstacleCorou = NormalObstacleCreate();
        StartCoroutine(ObstacleCorou);
        IEnumerator MathMoveCorou = MathMove();
        yield return StartCoroutine(MathMoveCorou);
        StopCoroutine(ObstacleCorou);
    }

    private IEnumerator NormalObstacleCreate()
    {
        while (true)
        {
            GameObject ob = Instantiate(Obstacle);
            ob.transform.position = transform.position;
            yield return new WaitForSeconds(0.02f);
        }
    }

    // 장애물을 소환할 때 움직이는 패턴입니다. 왼쪽, 오른쪽을 번갈아가며 이동합니다.
    private IEnumerator MathMove()
    {
        Vector3 targetPos = new Vector3(MinPos.x, MaxPos.y - MinPos.y, MaxPos.z - MinPos.z);
        while (true)
        {
            transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * MoveSpeed);

            if ((transform.position - targetPos).sqrMagnitude <= 0.1f)
            {
                transform.position = targetPos;
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }

        bool Right = true;
        float angle = 0;
        float xPos = transform.position.x;
        Vector3 tfp = gameObject.transform.position;
        for (int i = 0; i < 5; i++)
        {
            int randompt = Random.Range(0, 3);
            while (true)
            {
                if (Right) xPos += 0.2f;
                else xPos += -0.2f;
                angle += 0.1f;

                switch (randompt)
                {
                    case 0:
                        transform.position = tfp + new Vector3(xPos, Mathf.Sin(angle) * 3, 0);
                        break;
                    case 1:
                        transform.position = tfp + new Vector3(xPos, Mathf.Cos(angle) * 3, 0);
                        break;
                    case 2:
                        transform.position = tfp + new Vector3(xPos, Mathf.Tan(angle) * 3, 0);
                        break;
                }

                if (Right)
                {
                    if (transform.position.x >= MaxPos.x)
                    {
                        Right = false;
                        break;
                    }
                }
                else
                {
                    if (transform.position.x <= MinPos.x)
                    {
                        Right = true;
                        break;
                    }
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    // 보스의 HP가 0이되면 실행됩니다. 이펙트를 생성시키고 게임매니저의 ClearCallBack를 실행시켜 보스가 처치되었다는 것을 알립니다.
    private IEnumerator Death()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject effect = Instantiate(DeathEffect);
            effect.transform.position = gameObject.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            Destroy(effect, 2f);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

        GameManager.Instance.StageMgr.StartCoroutine(GameManager.Instance.StageMgr.ClearCallBack());
        Destroy(gameObject);
    }

    // 최소 백터와 최대 백터 사이에서 랜덤 값의 백터를 반환
    private Vector3 RandomVector3(Vector3 minPos, Vector3 maxPos)
    {
        Vector3 randomPos = new Vector3(
            Random.Range(minPos.x, maxPos.x),
            Random.Range(minPos.y, maxPos.y),
            Random.Range(minPos.z, maxPos.z));

        return randomPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PRocket")
        {
            _Hp -= other.gameObject.GetComponent<Rocket>().Damage;
        }

        if (other.gameObject.tag == "PBullet")
        {
            _Hp -= other.gameObject.GetComponent<PBullet>().Damage;
        }
    }
}

// 보스의 상태 구조를 만들기 위해 State Pattern을 사용했습니다.
public class BossState
{
    public IBoss State { get; set; }
    public CovidBoss Boss;

    public void Setting(CovidBoss boss)
    {
        this.State = new MovePattern();
        this.Boss = boss;
    }

    public void Process()
    {
        State.Process(this, Boss);
    }
}

public class AttackPattern : IBoss
{
    public void Process(BossState State, CovidBoss Boss)
    {
        Boss.StartCoroutine(Process_Coroutine(State, Boss));
    }

    public IEnumerator Process_Coroutine(BossState State, CovidBoss Boss)
    {
        IEnumerator AttackCoroutine = Boss.SelectAttackPattern();
        yield return Boss.StartCoroutine(AttackCoroutine);

        State.State = new MovePattern();
        State.Process();
    }
}

public class MovePattern : IBoss
{
    public void Process(BossState State, CovidBoss Boss)
    {
        Boss.StartCoroutine(Process_Coroutine(State, Boss));
    }
    public IEnumerator Process_Coroutine(BossState State, CovidBoss Boss)
    {
        IEnumerator MoveCoroutine = Boss.MovePattern();
        yield return Boss.StartCoroutine(MoveCoroutine);

        yield return new WaitForSeconds(2f);

        State.State = new AttackPattern();
        State.Process();
    }
}

public interface IBoss
{
    public abstract void Process(BossState State, CovidBoss Boss);
    public abstract IEnumerator Process_Coroutine(BossState State, CovidBoss Boss);
}