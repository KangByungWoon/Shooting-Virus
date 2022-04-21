using UnityEngine;
using KeyType = System.String;

[System.Serializable]
public class PoolObjectData
{
    public const int InitCount = 10;
    public const int MaxCount = 50;

    public KeyType key;
    public GameObject prefab;
    public int InitCreateCount = InitCount;
    public int MaxCreateCount = MaxCount;
}
