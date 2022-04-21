using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERocket : MonoBehaviour
{
    public Transform Target;
    public Vector3 TargetPosition;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] public float Speed;
    private bool isComplete = false;
    public int Damage;
    bool isAttack = false;

    [SerializeField] PoolObject m_Object;

    // 미사일의 타입에 따라 변수 값을 설정해줍니다.
    private void Start()
    {
        JsonSystem json = JsonSystem.Instance;
        switch (m_Object.key)
        {
            case "Bacteria":
                Damage = json.Information.Bacteria_Damage;
                Speed = json.Information.Bacteria_BulletSpeed;
                break;
            case "Germ":
                Damage = json.Information.Germ_Damage;
                Speed = json.Information.Germ_BulletSpeed;
                break;
            case "Cancer_Cells":
                Damage = json.Information.Cancer_Cells_Damage;
                Speed = json.Information.Cancer_Cells_BulletSpeed;
                break;
            case "Virus":
                Damage = json.Information.Virus_Damage;
                Speed = json.Information.Virus_BulletSpeed;
                break;
        }
    }

    private void OnEnable()
    {
        isComplete = false;
        isAttack = false;
    }

    // 플레이어를 타겟으로 설정하여 유도 이동을 하다가 근접하면 유도 이동을 해제하고 직선이동을 합니다.
    void Update()
    {
        try
        {
            if (!isComplete)
            {
                TargetPosition = Target.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime * Speed);
            transform.LookAt(TargetPosition);
            if (transform.position.z <= TargetPosition.z + 5f && !isComplete)
            {
                TargetPosition += transform.forward * 5;
                isComplete = true;
            }
            else if (transform.position == TargetPosition && isComplete)
            {
                RocketRelease();
            }
        }
        catch (MissingReferenceException)
        {
            RocketRelease();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isAttack)
        {
            isAttack = true;
            RocketRelease();
            other.GetComponentInParent<AirPlaneController>().InvinActive(Damage);
            PoolObject ex = ObjectPoolMgr.Instance.GetObject("Particle", gameObject.transform.position);
            ObjectPoolMgr.Instance.ReleaseObject(ex, 2f);
            Camera.main.GetComponent<CameraSystem>().CameraShake(0.25f, 0.3f);
        }

        if (other.tag == "Enemy" && other.gameObject.transform == Target)
        {
            isAttack = true;
            RocketRelease();
            PoolObject ex = ObjectPoolMgr.Instance.GetObject("Particle", gameObject.transform.position);
            ObjectPoolMgr.Instance.ReleaseObject(ex, 2f);
        }
    }

    public void RocketRelease()
    {
        ObjectPoolMgr.Instance.ReleaseObject(m_Object);
    }
}
