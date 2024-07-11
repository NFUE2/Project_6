
public abstract class MonsterAttack
{
    protected MonsterStateMachine stateMachine;

    protected MonsterAttack(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Attack();
}
