using Photon.Pun;
using System.Collections;
using UnityEngine;

public class P_TentaclypseRazorAttack : MonoBehaviourPun, IAttackPattern
{
    private GameObject boss;
    private P_Tentaclypse tentaclypse;
    private GameObject razorObject;
    private GameObject target;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        tentaclypse = boss.GetComponent<P_Tentaclypse>();
        razorObject = tentaclypse.razorObject;
    }

    public void ExecuteAttack()
    {
        Debug.Log($"레이저 공격 개시");
        target = tentaclypse.target;
        StartCoroutine(RazorObjectCoroutine());
    }

    private IEnumerator RazorObjectCoroutine()
    {
        for(int i = 0; i < 5; i++)
        {
            Vector3 razorPosition = target.transform.position;
            //var razor = Instantiate(razorObject, transform);
            var razor = PhotonNetwork.Instantiate(razorObject.name,Vector2.zero,Quaternion.identity);
            razor.transform.position = razorPosition;
            Vector3 currentRotation = razor.transform.eulerAngles;
            currentRotation.z = Random.Range(0, 180);
            razor.transform.eulerAngles = currentRotation;
            yield return new WaitForSeconds(1);
        }
    }
}