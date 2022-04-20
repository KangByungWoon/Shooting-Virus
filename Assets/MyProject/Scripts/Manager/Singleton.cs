using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object m_Lock = new object();
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //lock (m_Lock) // 멀티 스레드에서는 여러번 생성되는 것을 방지하기 위해 사용합니다.
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent(typeof(T)) as T;
                    singletonObject.name = typeof(T).ToString();
                }
            }
            return instance;
        }
    }
}
