public class GolemBoss : BossMonster
{
    public CharacterDataSO golemData;
    // ������SO
    private void Start()
    {
        // ������SO���� ���� ����
        // �ӽ� ������
        maxHp = golemData.maxHP;
        currentHp = maxHp;
        attackPower = golemData.attackDamage;
        defensePower = golemData.defence;
        moveSpeed = golemData.moveSpeed;
        // �ӽ� ������
        //hpBar.fillAmount = GetFillAmountHP();
    }
}