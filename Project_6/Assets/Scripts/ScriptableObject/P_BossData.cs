using UnityEngine;

[CreateAssetMenu(fileName = "Boss Data", menuName = "Scriptable Object/Boss Data", order = int.MaxValue)]
public class P_BossData : ScriptableObject
{
    // �̸�
    [SerializeField] private string bossName;
    public string BossName { get { return bossName; } }

    // ���ݷ�
    [SerializeField] private float bossPower;
    public float BossPower { get { return bossPower; } }

    // ü��
    [SerializeField] private float bossHp;
    public float BossHp {  get { return bossHp; } }

    // 
}