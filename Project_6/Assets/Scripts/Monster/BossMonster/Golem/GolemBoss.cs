public class GolemBoss : BossMonster
{
    // ������SO
    private void Start()
    {
        // ������SO���� ���� ����
        // �ӽ� ������
        maxHp = 10000;
        currentHp = maxHp;
        attackPower = 20;
        defensePower = 20;
        moveSpeed = 1;
        // �ӽ� ������
        hpBar.fillAmount = GetFillAmountHP();
    }
}