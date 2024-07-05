using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunSingleton<T> : MonoBehaviourPun where T : MonoBehaviourPun
{
    public static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                    instance = new GameObject(nameof(T), typeof(T)).GetComponent<T>();
            }

            return instance;
        }
    }


    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
