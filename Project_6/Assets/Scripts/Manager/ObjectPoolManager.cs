using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

[Serializable]
public class PoolData
{
    public string name;
    public int amount;

    public PoolData(string name, int amount)
    {
        this.name = name;
        this.amount = amount;
    }
}

[Serializable]
public class ObjectPoolData
{
    public List<PoolData> data;

    public ObjectPoolData() => data = new List<PoolData>();
}

public class ObjectPool
{
    Queue<GameObject> queue;

    public ObjectPool(PoolData data)
    {
        queue = new Queue<GameObject>();

        for(int i = 0; i < data.amount; i++)
        {
            GameObject g = PhotonNetwork.Instantiate(data.name,Vector3.zero,Quaternion.identity);
            queue.Enqueue(g);
        }
    }

    public GameObject Get() { return queue.Dequeue(); }
    public void Release(GameObject g) => queue.Enqueue(g);
}

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public Dictionary<string, ObjectPool> pool = new Dictionary<string, ObjectPool>();
    public GameObject Get(string name) { return pool[name].Get(); }
    public void Release(string name,GameObject g) => pool[name].Release(g);

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient) DataManager.instance.PoolDataLoad();
    }
}
