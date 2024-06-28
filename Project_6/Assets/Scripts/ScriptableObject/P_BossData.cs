using UnityEngine;

[CreateAssetMenu(fileName = "Boss Data", menuName = "Scriptable Object/Boss Data", order = int.MaxValue)]
public class P_BossData : ScriptableObject
{
    // 이름
    [SerializeField] private string bossName;
    public string BossName { get { return bossName; } }

    // 공격력
    [SerializeField] private float bossPower;
    public float BossPower { get { return bossPower; } }

    // 체력
    [SerializeField] private float bossHp;
    public float BossHp {  get { return bossHp; } }

    // 
}