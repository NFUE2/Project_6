public class TentaclypseBoss : BossMonster
{
    public CharacterDataSO tentaclypseData;

    private void Start()
    {
        maxHp = tentaclypseData.maxHP * SetMultiHP();
        currentHp = maxHp;
        attackPower = tentaclypseData.attackDamage;
        defensePower = tentaclypseData.defence;
        moveSpeed = tentaclypseData.moveSpeed;

        // hpBar.fillAmount = GetFillAmountHP();
    }
}