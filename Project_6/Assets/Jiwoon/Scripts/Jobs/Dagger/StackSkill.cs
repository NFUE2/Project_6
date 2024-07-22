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
            //stackText.text = "����: 0";
            lastActionTime = Time.time;
        }
        else
        {
            Debug.Log($"������ �����մϴ�. ���� ���� : {currentStack}");
        }
    }

    private void DealDamageWithStack()
    {
        int totalDamage = currentStack * damagePerStack;
        Debug.Log($"���� {currentStack}���� ����Ͽ� {totalDamage}�� �������� �������ϴ�.");

        // ���⿡ ������ �������� ������ �ڵ带 �߰�
    }

    public void IncreaseStack()
    {
        currentStack++;
        if (currentStack > maxStack)
        {
            currentStack = maxStack;
        }
        Debug.Log($"���� ����! ���� ���� : {currentStack}");
        //stackText.text = $"����: {currentStack}";
    }
}
