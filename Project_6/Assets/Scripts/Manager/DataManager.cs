using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : MonoBehaviour
{
    public AssetReference[] prefab;

    public async Task<bool> DataLoad()
    {
        DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;

        foreach (var p in prefab)
        {
            Addressables.LoadAssetAsync<GameObject>("Prefab");
            Task<GameObject> task = p.LoadAssetAsync<GameObject>().Task;

            await task;

            defaultPool.ResourceCache.Add(task.Result.name, task.Result);
            p.ReleaseAsset();
        }
        return true;
    }
}
