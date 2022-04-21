using UnityEngine;
using KeyType = System.String;

[DisallowMultipleComponent]
public class PoolObject : MonoBehaviour
{
    public KeyType key;

    public PoolObject Clone()
    {
        GameObject go = Instantiate(gameObject);
        if (!go.TryGetComponent(out PoolObject po))
        {
            po = go.AddComponent<PoolObject>();
        }
        go.SetActive(false);

        return po;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
