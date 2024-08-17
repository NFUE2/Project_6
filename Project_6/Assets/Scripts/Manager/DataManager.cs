using Photon.Pun;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
    #region DataInfo

    readonly string GSPath = "https://docs.google.com/spreadsheets/d/1f_3iBKFLpzz6HZ6yoejrlKIwtZYyc9FkAJov0Ei2b2A/";
    readonly string[] sheetPaths =
    {
        "0",
        //"2046122489"
    };

    #endregion

    Dictionary<int,ObjectSO> dataBase = new Dictionary<int, ObjectSO>();

    private static string path;
    public SaveData saveData;

    private void Start()
    {
        path = Application.persistentDataPath + "/SaveData.json";
        SaveDataCheck();
        StartCoroutine(WebDataLoad());
    }

    public async Task<bool> DataLoad()
    {
        DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;
        try
        {
            await Addressables.LoadAssetsAsync<GameObject>("Prefab", (g) =>
            {
                defaultPool.ResourceCache.Add(g.name, g);
            }).Task;

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private IEnumerator WebDataLoad()
    {
        for(int i = 0; i < sheetPaths.Length; i++)
        {
            UnityWebRequest www = UnityWebRequest.Get($"{GSPath}export?format=csv&gid={sheetPaths[i]}");
            yield return www.SendWebRequest();

            string data = www.downloadHandler.text;

            LoadCSV<PlayerDataSO>(data);
        }
    }

    private void LoadCSV<T>(string str) where T : ObjectSO
    {
        Dictionary<int,string> keys = new Dictionary<int,string>();
        Dictionary<int, string> varType = new Dictionary<int, string>();

        string[] datas = str.Trim().Split("\n"); //한줄로 분할

        //데이터 정리
        for(int i = 0; i < datas.Length; i++) //세로줄
        {
            List<string> strings = new List<string>();
            string[] d = datas[i].Trim().Split(",");

            for (int j = 0; j < d.Length; j++) //가로줄
            {
                if (i == 0) keys[j] = d[j];
                else if (i == 1) varType[j] = d[j];
                else
                {
                    StringBuilder json = new StringBuilder("");
                    int id = int.Parse(d[0]);

                    json.Append("{");

                    for (; j < keys.Count; j++)
                    {
                        if (j != 0) json.Append(",");
                        json.Append($"\"{keys[j]}\":");

                        if (varType[j] == "string") json.Append($"\"{d[j]}\"");
                        else json.Append($"{d[j]}");
                    }

                    json.Append("}");

                    try
                    {
                        T temp = ScriptableObject.CreateInstance<T>();
                        JsonUtility.FromJsonOverwrite(json.ToString(), temp);
                        dataBase[id] = temp;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
        }
    }
   
    public void SetData<T>(string[] data) where T : ObjectSO
    {

        //for (int j = 1; j < data.Length; j++)
        //{
        //    //string[] value = data[i].Split(",");
        //    //int id = int.Parse(value[0]);

        //    //dataBase[id] = new T(value);
        //}
    }

    public T GetData<T>(int id) where T : ObjectSO
    {
        if(dataBase.ContainsKey(id)) return dataBase[id] as T;

        return null;
    }

    void SaveDataCheck()
    {
        if (!File.Exists(path))
        {
            saveData = new SaveData();
            Save();
        }
        else
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);

            for (int i = 0; i < saveData.volums.Length; i++)
                SoundManager.instance.SetVolume((SourceType)i,saveData.volums[i]);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }

    //public async void PoolDataLoad()
    //{
    //    var data = await Addressables.LoadAssetAsync<TextAsset>("Data").Task;

    //    ObjectPoolData d = JsonUtility.FromJson<ObjectPoolData>(data.ToString());

    //    foreach (var pd in d.data)
    //    {
    //        ObjectPool pool = new ObjectPool(pd);
    //        ObjectPoolManager.instance.pool[pd.name] = pool;
    //    }
    //}

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
}