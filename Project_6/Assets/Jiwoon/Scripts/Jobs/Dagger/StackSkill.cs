using UnityEngine;
using TMPro;
using System.Collections;

public class StackSkill : SkillBase
{
    public int currentStack = 0;
    public int maxStack = 10;
    public int damagePerStack = 10;
    public TextMeshPro stackText;
    public PlayerDataSO PlayerData;

    private void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (currentStack > 0)
        {
            DealDamageWithStack();
            currentStack = 0;
            //stackText.text = "스택: 0";
            lastActionTime = Time.time;
        }
        else
        {
            Debug.Log($"스택이 부족합니다. 현재 스택 : {currentStack}");
        }
    }

    private void DealDamageWithStack()
    {
        int totalDamage = currentStack * damagePerStack;
        Debug.Log($"스택 {currentStack}개를 사용하여 {totalDamage}의 데미지를 입혔습니다.");

        // 여기에 적에게 데미지를 입히는 코드를 추가
    }

    public void IncreaseStack()
    {
        currentStack++;
        if (currentStack > maxStack)
        {
            currentStack = maxStack;
        }
        Debug.Log($"스택 증가! 현재 스택 : {currentStack}");
        //stackText.text = $"스택: {currentStack}";
    }
}
