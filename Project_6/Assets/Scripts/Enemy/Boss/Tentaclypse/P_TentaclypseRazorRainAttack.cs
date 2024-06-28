using System.Collections;
using System.Threading;
using UnityEngine;

public class P_TentaclypseRazorRainAttack : MonoBehaviour, IAttackPattern
{
    private GameObject boss;
    private P_Tentaclypse tentaclypse;
    private GameObject razorObject;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        tentaclypse = boss.GetComponent<P_Tentaclypse>();
        razorObject = tentaclypse.razorObject;
    }

    public void ExecuteAttack()
    {
        Debug.Log("·¹ÀÌÀú ¼¶¸ê °ø°Ý °³½Ã");
        StartCoroutine(RazorRainCoroutine());
    }

    private IEnumerator RazorRainCoroutine()
    {
        var vertRazor1 = Instantiate(razorObject, transform);
        vertRazor1.transform.position = new Vector3(-7.5f, 0, 0);
        var vertRazor2 = Instantiate(razorObject, transform);
        vertRazor2.transform.position = new Vector3(-5f, 0, 0);
        var vertRazor3 = Instantiate(razorObject, transform);
        vertRazor3.transform.position = new Vector3(-2.5f, 0, 0);
        var vertRazor4 = Instantiate(razorObject, transform);
        vertRazor4.transform.position = new Vector3(0, 0, 0);
        var vertRazor5 = Instantiate(razorObject, transform);
        vertRazor5.transform.position = new Vector3(2.5f, 0, 0);
        var vertRazor6 = Instantiate(razorObject, transform);
        vertRazor6.transform.position = new Vector3(5f, 0, 0);
        var vertRazor7 = Instantiate(razorObject, transform);
        vertRazor7.transform.position = new Vector3(7.5f, 0, 0);
        Vector3 currentRotation = vertRazor1.transform.eulerAngles;
        currentRotation.z = 90f;
        vertRazor1.transform.eulerAngles = currentRotation;
        vertRazor2.transform.eulerAngles = currentRotation;
        vertRazor3.transform.eulerAngles = currentRotation;
        vertRazor4.transform.eulerAngles = currentRotation;
        vertRazor5.transform.eulerAngles = currentRotation;
        vertRazor6.transform.eulerAngles = currentRotation;
        vertRazor7.transform.eulerAngles = currentRotation;
        yield return new WaitForSeconds(2f);
        var horiRazor1 = Instantiate(razorObject, transform);
        horiRazor1.transform.position = new Vector3(0, 3.5f, 0);
        var horiRazor2 = Instantiate(razorObject, transform);
        horiRazor2.transform.position = new Vector3(0, 1f, 0);
        var horiRazor3 = Instantiate(razorObject, transform);
        horiRazor3.transform.position = new Vector3(0, -1.5f, 0);
        var horiRazor4 = Instantiate(razorObject, transform);
        horiRazor4.transform.position = new Vector3(0, -4f, 0);
        currentRotation.z = 0;
        horiRazor1.transform.eulerAngles = currentRotation;
        horiRazor2.transform.eulerAngles = currentRotation;
        horiRazor3.transform.eulerAngles = currentRotation;
        horiRazor4.transform.eulerAngles = currentRotation;
    }
}