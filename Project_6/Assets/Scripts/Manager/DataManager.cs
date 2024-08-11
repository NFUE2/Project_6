using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TMPro;
using System.IO;
using System;
using System.Net.Http.Headers;

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
    public AssetReference[] prefab;

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
        foreach (var p in prefab)
        {
            Task<GameObject> task = p.LoadAssetAsync<GameObject>().Task;

            await task;

            //if (!defaultPool.ResourceCache.ContainsKey(task.Result.name))
            defaultPool.ResourceCache.Add(task.Result.name, task.Result);
            //p.ReleaseAsset();wd
        }

        return true;
    }

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