public class GolemBoss : BossMonster
{
    // 데이터SO
    private void Start()
    {
        // 데이터SO에서 스탯 설정
        // 임시 데이터
        maxHp = 10000;
        currentHp = maxHp;
        attackPower = 20;
        defensePower = 20;
        moveSpeed = 1;
        // 임시 데이터
        hpBar.fillAmount = GetFillAmountHP();
    }
}