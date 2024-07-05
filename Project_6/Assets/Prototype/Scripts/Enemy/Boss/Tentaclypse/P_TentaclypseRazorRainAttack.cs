using Photon.Pun;
using System.Collections;
using System.Threading;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class P_TentaclypseRazorRainAttack : MonoBehaviourPun, IAttackPattern
{
    private GameObject boss;
    private P_Tentaclypse tentaclypse;
    private GameObject razorObject;
    private Vector3[] verticalPositions;
    private Vector3[] horizontalPositions;

    PhotonView pv;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        tentaclypse = boss.GetComponent<P_Tentaclypse>();
        razorObject = tentaclypse.razorObject;
        InitPositions();
    }

    private void InitPositions()
    {
        verticalPositions = new Vector3[7];
        horizontalPositions = new Vector3[4];

        verticalPositions[0] = new Vector3(-7.5f, 0, 0);
        verticalPositions[1] = new Vector3(-5f, 0, 0);
        verticalPositions[2] = new Vector3(-2.5f, 0, 0);
        verticalPositions[3] = new Vector3(0, 0, 0);
        verticalPositions[4] = new Vector3(2.5f, 0, 0);
        verticalPositions[5] = new Vector3(5f, 0, 0);
        verticalPositions[6] = new Vector3(7.5f, 0, 0);

        horizontalPositions[0] = new Vector3(0, -4f, 0);
        horizontalPositions[1] = new Vector3(0, -1.5f, 0);
        horizontalPositions[2] = new Vector3(0, 1f, 0);
        horizontalPositions[3] = new Vector3(0, 3.5f, 0);
    }

    public void ExecuteAttack()
    {
        Debug.Log("·¹ÀÌÀú ¼¶¸ê °ø°Ý °³½Ã");
        StartCoroutine(RazorRainCoroutine());
    }

    private IEnumerator RazorRainCoroutine()
    {
        //var vertRazor1 = Instantiate(razorObject, transform);
        var vertRazor1 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name,Vector3.zero,Quaternion.identity);
        Vector3 currentRotation = vertRazor1.transform.eulerAngles;
        currentRotation.z = 90f;
        vertRazor1.transform.eulerAngles = currentRotation;
        vertRazor1.transform.position = verticalPositions[0];
        yield return new WaitForSeconds(0.2f);

        //var vertRazor2 = Instantiate(razorObject, transform);
        var vertRazor2 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        vertRazor2.transform.eulerAngles = currentRotation;
        vertRazor2.transform.position = verticalPositions[1];
        yield return new WaitForSeconds(0.2f);

        //var vertRazor3 = Instantiate(razorObject, transform);
        var vertRazor3 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        vertRazor3.transform.eulerAngles = currentRotation;
        vertRazor3.transform.position = verticalPositions[2];
        yield return new WaitForSeconds(0.2f);

        //var vertRazor4 = Instantiate(razorObject, transform);
        var vertRazor4 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        vertRazor4.transform.eulerAngles = currentRotation;
        vertRazor4.transform.position = verticalPositions[3];
        yield return new WaitForSeconds(0.2f);

        //var vertRazor5 = Instantiate(razorObject, transform);
        var vertRazor5 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        vertRazor5.transform.eulerAngles = currentRotation;
        vertRazor5.transform.position = verticalPositions[4];
        yield return new WaitForSeconds(0.2f);

        //var vertRazor6 = Instantiate(razorObject, transform);
        var vertRazor6 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        vertRazor6.transform.eulerAngles = currentRotation;
        vertRazor6.transform.position = verticalPositions[5];
        yield return new WaitForSeconds(0.2f);

        //var vertRazor7 = Instantiate(razorObject, transform);
        var vertRazor7 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        vertRazor7.transform.eulerAngles = currentRotation;
        vertRazor7.transform.position = verticalPositions[6];
        yield return new WaitForSeconds(1f);

        currentRotation.z = 0;
        //var horiRazor1 = Instantiate(razorObject, transform);
        var horiRazor1 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        horiRazor1.transform.eulerAngles = currentRotation;
        horiRazor1.transform.position = horizontalPositions[0];
        yield return new WaitForSeconds(0.35f);

        //var horiRazor2 = Instantiate(razorObject, transform);
        var horiRazor2 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        horiRazor2.transform.eulerAngles = currentRotation;
        horiRazor2.transform.position = horizontalPositions[1];
        yield return new WaitForSeconds(0.35f);

        //var horiRazor3 = Instantiate(razorObject, transform);
        var horiRazor3 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        horiRazor3.transform.eulerAngles = currentRotation;
        horiRazor3.transform.position = horizontalPositions[2];
        yield return new WaitForSeconds(0.35f);

        //var horiRazor4 = Instantiate(razorObject, transform);
        var horiRazor4 = PhotonNetwork.Instantiate("Prototype/" + razorObject.name, Vector3.zero, Quaternion.identity);
        horiRazor4.transform.eulerAngles = currentRotation;
        horiRazor4.transform.position = horizontalPositions[3];
        
    }
}