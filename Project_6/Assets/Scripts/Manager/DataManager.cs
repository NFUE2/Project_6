using Photon.Pun;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;
using System;
using System.Threading.Tasks;

[Serializable]
public class SaveData
{
    public float[] volums;
    public SaveData()
    {
        volums = new float[2] { 1.0f,1.0f};
    }
}

public class DataManager : Singleton<DataManager>
{
    private static string path;

    public SaveData data;

    private void Start()
    {
        path = Application.persistentDataPath + "/SaveData.json";
        SaveDataCheck();
    }

    public async Task<bool> DataLoad()
    {
        DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;

        await Addressables.LoadAssetsAsync<GameObject>("Prefab", (g) =>
        {
            defaultPool.ResourceCache.Add(g.name, g);
        }).Task;

        return true;
    }

    public async void PoolDataLoad()
    {
        var data = await Addressables.LoadAssetAsync<TextAsset>("Data").Task;

        ObjectPoolData d = JsonUtility.FromJson<ObjectPoolData>(data.ToString());

        foreach (var pd in d.data)
        {
            ObjectPool pool = new ObjectPool(pd);
            ObjectPoolManager.instance.pool[pd.name] = pool;
        }
    }

    //public async Task<bool> DataLoad()
    //{
    //    DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;

    //    foreach (var p in prefab)
    //    {
    //        Task<GameObject> task = p.LoadAssetAsync<GameObject>().Task;

    //        //GameObject g = await p.LoadAssetAsync<GameObject>().Task;

    //        await task;

    //        //if (!defaultPool.ResourceCache.ContainsKey(task.Result.name))
    //        defaultPool.ResourceCache.Add(task.Result.name, task.Result);

    //        ObjectPool obj = new ObjectPool(task.Result.name, 1);
    //        ObjectPoolManager.instance.pool[task.Result.name] = obj;

    //        await Task.Run(() => {  });
    //    }

    //    return true;
    //}

    void SaveDataCheck()
    {
        if (!File.Exists(path))
        {
            data = new SaveData();
            Save();
        }
        else
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);

            for (int i = 0; i < data.volums.Length; i++)
                SoundManager.instance.SetVolume((SourceType)i,data.volums[i]);
        }
    }

    public  void Save()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
}