using Photon.Pun;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject monsterPrefab;
    private GameObject monster;
    MonsterCondition monsterCondition;
    public int spawnTime;

    private void Start()
    {
        //monster = PhotonNetwork.Instantiate("Monster/" + monsterPrefab.name, transform.position, Quaternion.identity);
        monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
        monsterCondition = monster.GetComponent<MonsterCondition>();
        monsterCondition.OnDie += ReSpawnCoolTime;
    }

    private void ReSpawnCoolTime()
    {
        Invoke(nameof(ReSpawn),spawnTime);
    }

    private void ReSpawn()
    {
        monster.transform.position = transform.position;
        monster.SetActive(true);
    }
}
