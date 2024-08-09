
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null) CreateInstance();
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
                if (Application.isPlaying) Destroy(objects[i].gameObject);
                else DestroyImmediate(objects[i].gameObject);
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
        else DontDestroyOnLoad(gameObject);
    }
}
