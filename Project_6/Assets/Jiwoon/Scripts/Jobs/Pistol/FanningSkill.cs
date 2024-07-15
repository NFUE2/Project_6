using UnityEngine;
using System.Collections;
using TMPro;
using Photon.Pun;

public class FanningSkill : MonoBehaviour
{
    //이 부분은 PistolPlayer에서 처리 가능할 것 같습니다
    public Transform attackPoint;
    public GameObject attackPrefab;
    public bool IsFanningReady { get; private set; }
    //===================================

    // 상위클래서에서 처리
    public PlayerData PlayerData; 
    private float lastActionTime;
    private TextMeshProUGUI cooldownText;
    //====================================


    //쿨타임표기는 상위클래스에서 처리
    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }
    //========================

    //오버라이드
    public void UseSkill()
    {
        if (IsFanningReady) return;
        if (Time.time - lastActionTime < PlayerData.SkillQCooldown) return;

        IsFanningReady = true;
        StartCoroutine(Fanning());
    }
    
    //Interface 클래스를 하나 만들어서 상속받아서 따로 작동하도록 해주세요
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

    //상위클래스에서 처리
    private void Update()
    {
        if (cooldownText != null)
        {
            if (Time.time - lastActionTime >= PlayerData.SkillQCooldown)
            {
                cooldownText.text = "Q스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
            }
            else
            {
                float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastActionTime);
                cooldownText.text = $"{remainingTime:F1}"; // 쿨타임 텍스트 갱신
            }
        }
    }
}
