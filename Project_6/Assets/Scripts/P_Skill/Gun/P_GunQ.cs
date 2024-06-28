using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class P_GunQ : MonoBehaviour, P_ISkill
{
    //�÷��̾� ��ü �����;���
    public GameObject bullet;
    private Transform hand;
    public bool fanningReady { get; private set; }

    private void Awake()
    {
        hand = GetComponent<P_SkillTest>().hand;
    }

    public void SkillAction()
    {
        Debug.Log(1);
        //������ ��ũ��Ʈ �ʿ�
        if (fanningReady) return;

        fanningReady = true;
        StartCoroutine(Fanning());
    }

    IEnumerator Fanning()
    {
        while(!Input.GetMouseButtonDown(0))
            yield return null;

        for(int i = 0; i < 6; i++)
        {
            float fireAngle = Random.Range(-3f, 3f);
            GameObject go = Instantiate(bullet, hand.GetChild(0).position, Quaternion.identity);
            go.transform.localEulerAngles = hand.localEulerAngles + new Vector3(0, 0,fireAngle);

            yield return new WaitForSeconds(0.1f);
        }
        fanningReady = false;
    }
}
