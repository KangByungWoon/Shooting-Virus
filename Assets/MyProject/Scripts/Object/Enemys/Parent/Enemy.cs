using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isTarget;
    public GameObject RocketObj;

    [SerializeField] protected GameObject Explosion;
    [SerializeField] protected BezierCurve bezier;
    [SerializeField] protected GameObject ERocket;
    [SerializeField] protected GameObject XMark;

    public ObjectPool.PoolType EnemyType;
    public int GiveScore;
    public float GiveExp;

    protected int Hp;
    protected int Damage;
    protected float MoveSpeed;
    protected bool isDie = false;

    protected JsonSystem json;
    Context context;

    private void Start()
    {
        StartSetting();
    }

    // 적의 타입에 따라 초기 변수값을 설정해줍니다.
    protected virtual void StartSetting()
    {
        if (json == null)
            json = JsonSystem.Instance;
    }

    public void Setting()
    {
        isDie = false;
        isTarget = false;
        XMark.SetActive(false);
        SetContext();
        StartActivity();
    }

    public void SetContext()
    {
        context = new Context(this);
    }

    public void EixtEnemy()
    {
        StopAllCoroutines();

        context.State = new ExitState();
        context.Activity();
    }

    private void StartActivity()
    {
        context.Activity();
        StartCoroutine(TargetRelease());
    }

    private IEnumerator TargetRelease()
    {
        while (true)
        {
            isTarget = false;
            XMark.SetActive(false);
            RocketObj = null;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Move(bool Exit)
    {
        if (!Exit)
        {
            transform.rotation = Quaternion.identity;
            SetMovePoint();
        }
        else
        {
            SetExitMovePoint();
        }
        StartCoroutine(MoveBezier(Exit));
    }
    public void SetMovePoint()
    {
        bezier.P1 = gameObject.transform.position;
        bezier.P2 = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), Random.Range(15, 100));
        bezier.P3 = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), Random.Range(15, 100));
        bezier.P4 = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), Random.Range(15, 100));
        bezier.Value = 0f;
    }
    public void SetExitMovePoint()
    {
        bezier.P1 = gameObject.transform.position;
        bezier.P2 = new Vector3(Random.Range(GameManager.Instance.Player.transform.position.x - 10, GameManager.Instance.Player.transform.position.x + 10
            ), Random.Range(GameManager.Instance.Player.transform.position.y - 5, GameManager.Instance.Player.transform.position.y + 5)
            , -5);
        bezier.P3 = new Vector3(Random.Range(GameManager.Instance.Player.transform.position.x - 10, GameManager.Instance.Player.transform.position.x + 10
            ), Random.Range(GameManager.Instance.Player.transform.position.y - 5, GameManager.Instance.Player.transform.position.y + 5)
            , -5);
        bezier.P4 = new Vector3(Random.Range(GameManager.Instance.Player.transform.position.x - 10, GameManager.Instance.Player.transform.position.x + 10
            ), Random.Range(GameManager.Instance.Player.transform.position.y - 5, GameManager.Instance.Player.transform.position.y + 5)
            , -5);
        bezier.Value = 0f;
    }

    public void Shooting(bool isRed = false)
    {
        GameObject rocket = null;
        switch (EnemyType)
        {
            case ObjectPool.PoolType.Bacteria:
                rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.BacteriaRockets, transform.position);
                break;
            case ObjectPool.PoolType.Germ:
                rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.GermRockets, transform.position);
                break;
            case ObjectPool.PoolType.Virus:
                rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.VirusRockets, transform.position);
                break;
            case ObjectPool.PoolType.Cancer_Cells:
                rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.Cancer_CellsRockets, transform.position);
                break;
        }

        if (!isRed)
        {
            rocket.GetComponent<ERocket>().Target = GameManager.Instance.Player.transform;
        }
        else
        {
            var red = FindObjectOfType<RedBlood_Cells>();
            if (red != null)
                rocket.GetComponent<ERocket>().Target = red.transform;
            else
                rocket.GetComponent<ERocket>().Target = GameManager.Instance.Player.transform;
        }
    }

    // 적의 이동을 베지어곡선으로 구현했습니다. Lerp / Slerp 등 다른 벡터기반 이동보다 자유로운 이동이가능합니다.
    public IEnumerator MoveBezier(bool Exit)
    {
        while (true)
        {
            bezier.Value += Exit ? MoveSpeed * 2 : MoveSpeed;
            if (bezier.Value >= 1)
            {
                bezier.Value = 1;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        if (Exit)
        {
            Die();
            GameManager.Instance.Gp += Damage;
        }
    }

    public void OnMark()
    {
        XMark.SetActive(true);
    }

    public virtual void Die()
    {
        GameManager.Instance.KillEnemy++;
        if (EnemyType != ObjectPool.PoolType.Leukocyte && EnemyType != ObjectPool.PoolType.RedBlood_Cells)
        {
            int random = Random.Range(0, 100);
            if (random < JsonSystem.Instance.Information.Leukocyte_SpawnTimer)
            {
                random = Random.Range(0, 100);
                if (random < JsonSystem.Instance.Information.Leukocyte_SpawnPer)
                {
                    ObjectPool.Instance.GetObject(ObjectPool.Instance.Leukocytes, gameObject.transform.position);
                }
                else
                {
                    ObjectPool.Instance.GetObject(ObjectPool.Instance.RedBlood_Cellses, gameObject.transform.position);
                }
            }
        }

        switch (EnemyType)
        {
            case ObjectPool.PoolType.Bacteria:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Bacterias, gameObject);
                break;
            case ObjectPool.PoolType.Germ:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Germs, gameObject);
                break;
            case ObjectPool.PoolType.Virus:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Viruses, gameObject);
                break;
            case ObjectPool.PoolType.Cancer_Cells:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Cancer_Cellses, gameObject);
                break;
            case ObjectPool.PoolType.Leukocyte:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Leukocytes, gameObject);
                break;
            case ObjectPool.PoolType.RedBlood_Cells:
                ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.RedBlood_Cellses, gameObject);
                break;
        }
    }

    public void TargetSetting()
    {
        if (gameObject != RocketObj.GetComponent<Rocket>().Target.gameObject)
        {
            isTarget = false;
            XMark.SetActive(false);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!isDie)
        {
            if (other.gameObject.tag == "PRocket")
            {
                if (other.GetComponent<Rocket>().Target == gameObject.transform)
                {
                    Hp -= other.GetComponent<Rocket>().Damage;
                    if (Hp <= 0)
                    {
                        Die();
                        GameManager.Instance.Score += GiveScore;
                        GameManager.Instance.Player.GetComponent<AirPlaneController>()._Exp += GiveExp;
                        isDie = true;
                    }
                }
            }

            if (other.gameObject.tag == "PBullet")
            {
                Hp -= other.gameObject.GetComponent<PBullet>().Damage;
                if (Hp <= 0)
                {
                    Die();
                    GameManager.Instance.Score += GiveScore;
                    GameManager.Instance.Player.GetComponent<AirPlaneController>()._Exp += GiveExp;
                    isDie = true;
                }
            }

            if (other.gameObject.tag == "Player")
            {
                Die();
                isDie = true;
                other.GetComponentInParent<AirPlaneController>().InvinActive(Damage);
            }
        }
    }
}

// 적의 상태 구조를 만들기 위해 State Pattern을 사용했습니다.
public class Context
{
    public IState State { get; set; }
    Enemy enemy;

    public Context(Enemy enemy)
    {
        this.enemy = enemy;
        this.State = new MoveState();
    }

    public void Activity()
    {
        enemy.StartCoroutine(this.State.Activity(this, enemy));
    }
}

public class MoveState : IState
{
    public IEnumerator Activity(Context ctx, Enemy enemy)
    {
        enemy.Move(false);

        yield return new WaitForSeconds(Random.Range(2f, 3.5f));

        ctx.State = new AttackState();
        ctx.Activity();
    }
}

public class AttackState : IState
{
    public IEnumerator Activity(Context ctx, Enemy enemy)
    {
        if (enemy.EnemyType == ObjectPool.PoolType.Leukocyte || enemy.EnemyType == ObjectPool.PoolType.RedBlood_Cells ||
            enemy.EnemyType == ObjectPool.PoolType.Bacteria)
        {
            int random = Random.Range(0, 100);
            if (random <= 75)
            {
                ctx.State = new ExitState();
            }
            else
            {
                ctx.State = new MoveState();
            }
            ctx.Activity();
        }
        else
        {
            int random = Random.Range(0, 100);
            if (random <= 20)
            {
                enemy.Shooting(true);
            }
            else
            {
                enemy.Shooting();
            }

            yield return new WaitForSeconds(Random.Range(5f, 8f));
            int random2 = Random.Range(0, 100);
            if (random2 <= 75)
            {
                ctx.State = new ExitState();
            }
            else
            {
                ctx.State = new MoveState();
            }
            ctx.Activity();
        }
    }
}

public class ExitState : IState
{
    public IEnumerator Activity(Context ctx, Enemy enemy)
    {
        enemy.Move(true);
        yield return null;
    }
}

public interface IState
{
    public abstract IEnumerator Activity(Context ctx, Enemy enemy);
}