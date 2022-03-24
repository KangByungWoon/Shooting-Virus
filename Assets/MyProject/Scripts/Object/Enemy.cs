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

    public virtual void Start()
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
