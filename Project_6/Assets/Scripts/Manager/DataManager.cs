using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TMPro;
public class DataManager : MonoBehaviour
{
    public AssetReference[] prefab;
    public TextMeshProUGUI text;
    public async Task<bool> DataLoad()
    {
        DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;
        foreach (var p in prefab)
        {
            //Addressables.LoadAssetAsync<GameObject>("Prefab");
            Task<GameObject> task = p.LoadAssetAsync<GameObject>().Task;

            await task;

            //if (!defaultPool.ResourceCache.ContainsKey(task.Result.name))
            defaultPool.ResourceCache.Add(task.Result.name, task.Result);
            text.text = task.Result.name;
            //p.ReleaseAsset();wd
        }

        return true;
    }
}
