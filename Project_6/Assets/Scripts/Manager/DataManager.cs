using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    public AssetReference[] prefab;

    private async void Start()
    {
        DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;

        foreach (var p in prefab)
        {
            Addressables.LoadAssetAsync<GameObject>("Prefab");
            Task<GameObject> task = p.LoadAssetAsync<GameObject>().Task;

            await task;

            Debug.Log(task.Result.name);

            defaultPool.ResourceCache.Add(task.Result.name, task.Result);
            p.ReleaseAsset();
        }
    }
}
