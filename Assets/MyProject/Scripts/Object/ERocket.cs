using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERocket : MonoBehaviour
{
    public Vector3 Target;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] protected float Speed;
    private bool isComplete = false;

    void Update()
    {
        try
        {
            transform.position = Vector3.MoveTowards(transform.position, Target + new Vector3(0,0,0), Time.deltaTime * Speed);
            transform.LookAt(Target);
            if(transform.position.z <= Target.z + 0.1f && !isComplete)
            {
                transform.position = Target;
                Target += new Vector3(0, 0, -5f);
                isComplete = true;
            }
            else if(transform.position.z <= Target.z + 0.1f && isComplete)
            {
                Destroy(gameObject);
            }
        }
        catch (MissingReferenceException)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject ex = Instantiate(Explosion);
            ex.transform.position = gameObject.transform.position;
            Destroy(gameObject);
            Destroy(ex, 2f);
        }
    }
}
