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

    public void SetFirstMovePoint()
    {
        bezier.P1 = gameObject.transform.position;
        bezier.P2 = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), Random.Range(15, 100));
        bezier.P3 = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), Random.Range(15, 100));
        bezier.P4 = new Vector3(Random.Range(-30, 30), Random.Range(10, 30), Random.Range(15, 100));
        Move();
    }

    void Shooting()
    {
        GameObject rocket = Instantiate(ERocket);
        rocket.transform.position = transform.position;
        rocket.GetComponent<ERocket>().Target = GameManager.Instance.Player.transform.position;
    }

    private void Move()
    {
        StartCoroutine(MoveBezier());
    }

    public void OnMark()
    {
        XMark.SetActive(true);
    }

    public IEnumerator MoveBezier()
    {
        while (true)
        {
            bezier.Value += MoveSpeed;
            if (bezier.Value >= 1)
            {
                bezier.Value = 1;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        Shooting();
    }

    public virtual void Die()
    {
        GameObject ex = Instantiate(Explosion);
        ex.transform.position = gameObject.transform.position;
        Destroy(gameObject);
        Destroy(ex, 2f);
        Debug.Log("Die Callback");
    }
}
