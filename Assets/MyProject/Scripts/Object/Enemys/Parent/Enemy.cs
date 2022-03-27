using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isTarget;
    public GameObject RocketObj;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] protected BezierCurve bezier;
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected GameObject ERocket;
    [SerializeField] protected GameObject XMark;
    public int Damage;
    public ObjectPool.PoolType EnemyType;
    public int GiveScore;
    protected bool isDie = false;

    Context context;

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

    private void StartActivity()
    {
        context.Activity();
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

    public void Shooting()
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

        rocket.GetComponent<ERocket>().Target = GameManager.Instance.Player.transform;
    }

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
        if (EnemyType != ObjectPool.PoolType.Leukocyte && EnemyType != ObjectPool.PoolType.RedBlood_Cells)
        {
            int random = Random.Range(0, 100);
            if (random < 5)
            {
                random = Random.Range(0, 100);
                if (random < 50)
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

        Debug.Log("Die Callback");
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
                    Die();
                    GameManager.Instance.Score += GiveScore;
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
        }
        else
        {
            enemy.Shooting();

            yield return new WaitForSeconds(Random.Range(5f, 8f));
        }

        ctx.Activity();
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