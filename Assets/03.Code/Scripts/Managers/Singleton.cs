using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);

            return;
        }
        else
        {
            _instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
