using System.Collections;
using UnityEngine;

public class P_TentaclypseRazorAttack : MonoBehaviour, IAttackPattern
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
        Debug.Log($"������ ���� ����");
        target = tentaclypse.target;
        StartCoroutine(RazorObjectCoroutine());
    }

    private IEnumerator RazorObjectCoroutine()
    {
        for(int i = 0; i < 10; i++)
        {
            Vector3 razorPosition = target.transform.position;
            var razor = Instantiate(razorObject, transform);
            razor.transform.position = razorPosition;
            Vector3 currentRotation = razor.transform.eulerAngles;
            currentRotation.z = Random.Range(0, 180);
            razor.transform.eulerAngles = currentRotation;
            yield return new WaitForSeconds(1);
        }
    }
}