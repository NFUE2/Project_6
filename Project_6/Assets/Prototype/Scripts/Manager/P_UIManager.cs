using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_UIManager : MonoBehaviour
{
    [Header("Boss")]
    public GameObject boss;
    public P_BossMonster bossMonster;

    public Image bossHPBar;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossMonster = boss.GetComponent<P_BossMonster>();
    }

    private void Update()
    {
        ShowBossHPBar();
    }

    private void ShowBossHPBar()
    {
        bossHPBar.fillAmount = bossMonster.bossHp / bossMonster.maxHp;
//        Debug.Log();
    }
}
