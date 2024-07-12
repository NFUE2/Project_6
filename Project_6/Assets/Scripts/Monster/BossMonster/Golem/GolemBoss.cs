public class GolemBoss : BossMonster
{
    public CharacterDataSO golemData;
    // 데이터SO
    private void Start()
    {
        // 데이터SO에서 스탯 설정
        // 임시 데이터
        maxHp = golemData.maxHP;
        currentHp = maxHp;
        attackPower = golemData.attackDamage;
        defensePower = golemData.deffenceDamage;
        moveSpeed = golemData.moveSpeed;
        // 임시 데이터
        hpBar.fillAmount = GetFillAmountHP();
    }
}