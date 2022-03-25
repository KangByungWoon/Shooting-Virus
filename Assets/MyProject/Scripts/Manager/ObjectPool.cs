using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public enum PoolType
    {
        Bacteria,
        Germ,
        Virus,
        Cancer_Cells,
        ERocket,
        PRocket,
        Particle
    };

    [Header("Pools")]
    public Queue<GameObject> Bacterias = new Queue<GameObject>();
    public Queue<GameObject> Germs = new Queue<GameObject>();
    public Queue<GameObject> Viruses = new Queue<GameObject>();
    public Queue<GameObject> Cancer_Cellses = new Queue<GameObject>();
    public Queue<GameObject> ERockets = new Queue<GameObject>();
    public Queue<GameObject> PRockets = new Queue<GameObject>();
    public Queue<GameObject> Particles = new Queue<GameObject>();

    [Header("Prefabs")]
    [SerializeField] private GameObject Bacteria;
    [SerializeField] private GameObject Germ;
    [SerializeField] private GameObject Virus;
    [SerializeField] private GameObject Cancer_Cells;
    [SerializeField] private GameObject ERocket;
    [SerializeField] private GameObject PRocket;
    [SerializeField] private GameObject Particle;

    [Header("PoolParent")]
    [SerializeField] private Transform BacteriaPool;
    [SerializeField] private Transform GermPool;
    [SerializeField] private Transform VirusPool;
    [SerializeField] private Transform Cancer_CellsPool;
    [SerializeField] private Transform ERocketPool;
    [SerializeField] private Transform PRocketPool;
    [SerializeField] private Transform ParticlePool;

    private void Awake()
    {
        Instance = this;
        Setting();
    }

    private void Setting()
    {
        AddObject(Bacteria, Bacterias, BacteriaPool);
        AddObject(Germ, Germs, GermPool);
        AddObject(Virus, Viruses, VirusPool);
        AddObject(Cancer_Cells, Cancer_Cellses, Cancer_CellsPool);
        AddObject(ERocket, ERockets, ERocketPool);
        AddObject(PRocket, PRockets, PRocketPool);
        AddObject(Particle, Particles, ParticlePool);
    }

    private void AddObject(GameObject obj, Queue<GameObject> objectPool, Transform pool_parent, int addValue = 100)
    {
        for (int i = 0; i < addValue; i++)
        {
            GameObject tempObj = Instantiate(obj, pool_parent);
            objectPool.Enqueue(tempObj);
            tempObj.SetActive(false);
        }
    }

    public GameObject GetObject(Queue<GameObject> objectPool, Vector3 pos)
    {
        GameObject obj = objectPool.Dequeue();
        obj.transform.position = pos;
        obj.SetActive(true);
        return obj;
    }


    public void ReleaseObject(Queue<GameObject> objectPool, GameObject m_go, float waitsecond = 0)
    {
        StartCoroutine(ReleaseObject_Coroutine(objectPool, m_go, waitsecond));
    }
    public IEnumerator ReleaseObject_Coroutine(Queue<GameObject> objectPool, GameObject m_go, float waitsecond)
    {
        yield return new WaitForSeconds(waitsecond);

        objectPool.Enqueue(m_go);
        m_go.SetActive(false);
    }

}