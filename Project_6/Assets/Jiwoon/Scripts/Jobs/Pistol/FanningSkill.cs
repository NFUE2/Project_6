using UnityEngine;
using System.Collections;
using TMPro;
using Photon.Pun;

public class FanningSkill : MonoBehaviour
{
    //�� �κ��� PistolPlayer���� ó�� ������ �� �����ϴ�
    public Transform attackPoint;
    public GameObject attackPrefab;
    public bool IsFanningReady { get; private set; }
    //===================================

    // ����Ŭ�������� ó��
    public PlayerData PlayerData; 
    private float lastActionTime;
    private TextMeshProUGUI cooldownText;
    //====================================


    //��Ÿ��ǥ��� ����Ŭ�������� ó��
    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }
    //========================

    //�������̵�
    public void UseSkill()
    {
        if (IsFanningReady) return;
        if (Time.time - lastActionTime < PlayerData.SkillQCooldown) return;

        IsFanningReady = true;
        StartCoroutine(Fanning());
    }
    
    //Interface Ŭ������ �ϳ� ���� ��ӹ޾Ƽ� ���� �۵��ϵ��� ���ּ���
    private IEnumerator Fanning()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;

        for (int i = 0; i < 6; i++)
        {
            float fireAngle = Random.Range(-3f, 3f);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            GameObject go = PhotonNetwork.Instantiate(attackPrefab.name, transform.position, Quaternion.identity);
            go.transform.localEulerAngles = new Vector3(0, 0, angle + fireAngle);

            yield return new WaitForSeconds(0.1f);
        }

        IsFanningReady = false;
        lastActionTime = Time.time;
    }

    //����Ŭ�������� ó��
    private void Update()
    {
        if (cooldownText != null)
        {
            if (Time.time - lastActionTime >= PlayerData.SkillQCooldown)
            {
                cooldownText.text = "Q��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
            }
            else
            {
                float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastActionTime);
                cooldownText.text = $"{remainingTime:F1}"; // ��Ÿ�� �ؽ�Ʈ ����
            }
        }
    }
}
