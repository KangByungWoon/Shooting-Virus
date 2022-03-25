using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isTarget;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] protected BezierCurve bezier;
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected GameObject ERocket;
    [SerializeField] protected GameObject XMark;
    Context context;

    public virtual void Start()
    {
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
            SetMovePoint();
        else
        {
            SetExitMovePoint();
            transform.LookAt(transform.forward * -1);
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
        GameObject rocket = ObjectPool.Instance.GetObject(ObjectPool.Instance.ERockets, transform.position);
        rocket.GetComponent<ERocket>().Target = GameManager.Instance.Player.transform;
    }

    public IEnumerator MoveBezier(bool Exit)
    {
        while (true)
        {
            bezier.Value += Exit ? MoveSpeed * 2 :  MoveSpeed;
            if (bezier.Value >= 1)
            {
                bezier.Value = 1;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        if (Exit)
            Die();
    }

    public void OnMark()
    {
        XMark.SetActive(true);
    }

    public virtual void Die()
    {
        ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Bacterias, gameObject);
        GameObject ex =  ObjectPool.Instance.GetObject(ObjectPool.Instance.Particles, gameObject.transform.position);
        ObjectPool.Instance.ReleaseObject(ObjectPool.Instance.Particles, ex, 2f);
        Debug.Log("Die Callback");
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
        //enemy.Shooting();

        yield return new WaitForSeconds(Random.Range(5f, 8f));

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