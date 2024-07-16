using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Create();
                return _instance;
            }
        }

        protected static void Create()
        {
            if (_instance == null)
            {
                T[] objects = FindObjectsOfType<T>();

                if (objects.Length > 0)
                {
                    _instance = objects[0];

                    for (int i = 1; i < objects.Length; ++i)
                    {
                        if (Application.isPlaying)
                            Destroy(objects[i].gameObject);
                        else
                            DestroyImmediate(objects[i].gameObject);
                    }
                }
                else
                {
                    GameObject go = new GameObject(string.Format("{0}", typeof(T).Name));
                    _instance = go.AddComponent<T>();
                }
            }
        }
    }
}