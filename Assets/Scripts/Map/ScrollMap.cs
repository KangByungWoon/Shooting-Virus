using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMap : MonoBehaviour
{
    // z : -90이 되면 z : 90으로 이동
    [Header("ScrollMap Object")]
    [SerializeField] private GameObject[] ScrollMapObj = new GameObject[2];

    [Header("Move Information")]
    [SerializeField] private float Map_MoveSpeed;
    [SerializeField] private float ResetPosition;
    [SerializeField] private float EndPosition;

    private void Start()
    {
        Debug.Log(ScrollMapObj[0].transform.position);
        Debug.Log(ScrollMapObj[1].transform.position);
    }

    void Update()
    {
        foreach (GameObject obj in ScrollMapObj)
        {
            obj.transform.position += Vector3.back * Map_MoveSpeed;
            if (obj.transform.position.z <= EndPosition)
            {
                Debug.Log(obj.transform.position.z);
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, ResetPosition);
            }
        }
    }
}
