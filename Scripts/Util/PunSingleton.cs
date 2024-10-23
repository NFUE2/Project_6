using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PunSingleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    public static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                CreateInstance();
            //{
            //    instance = FindObjectOfType<T>();

                //    if (instance == null)
                //        instance = new GameObject(nameof(T), typeof(T)).GetComponent<T>();
                //}

            return instance;
        }
    }

    protected static void CreateInstance()
    {
        T[] objects = FindObjectsOfType<T>();

        if (objects.Length > 0)
        {
            instance = objects[0];

            for (int i = 1; i < objects.Length; i++)
            {
                if (Application.isPlaying) Destroy(objects[i]);
                else DestroyImmediate(objects[i]);
            }
        }
        else
        {
            instance = new GameObject(nameof(T), typeof(T)).GetComponent<T>();
        }
    }


    public virtual void Awake()
    {
        CreateInstance();

        if (instance != this) Destroy(gameObject);
        else DontDestroyOnLoad(this);
    }
}
