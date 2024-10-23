//using Photon.Pun;
//using UnityEngine;

//public class MonsterSpawn : MonoBehaviour
//{
//    public GameObject monsterPrefab;
//    private GameObject monster;
//    public int spawnTime;

//    //public Transform monsterList;

//    private void Start()
//    {
//        if (PhotonNetwork.IsMasterClient)
//        {
//            monster = PhotonNetwork.Instantiate("Monster/" + monsterPrefab.name, transform.position, Quaternion.identity);

//            MonsterCondition monsterCondition;
//            monsterCondition = monster.GetComponent<MonsterCondition>();
//            //monsterCondition.OnDie += ReSpawnCoolTime;
//        }
//        //monster.transform.SetParent(monsterList);
//        //monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
//    }

//    //private void ReSpawnCoolTime()
//    //{
//    //    Invoke(nameof(ReSpawn), spawnTime);
//    //}

//    //private void ReSpawn()
//    //{
//    //    monster.transform.position = transform.position;
//    //    monster.SetActive(true);
//    //}
//}
