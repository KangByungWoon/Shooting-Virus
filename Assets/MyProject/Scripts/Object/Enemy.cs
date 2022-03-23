using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isTarget;
    public GameObject Explosion;
    public BezierCurve bezier;
    public float MoveSpeed;

    public virtual void Start()
    {
        StartCoroutine(MoveBezier());
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
