using UnityEngine;


public class BossStateMachine : MonoBehaviour
{
    protected IBossState currentState;

    public BossMonster BossMonster { get; set; }

    public float MovementSpeed { get; set; }
    public bool IsAttacking { get; set; }

    public BossIdleState IdleState { get; set; }
    public BossAttackState AttackState { get; set; }
    public BossDieState DieState { get; set; }

    private void Start()
    {
        BossMonster = transform.GetComponent<BossMonster>();
        MovementSpeed = BossMonster.moveSpeed;

        IsAttacking = false;

        IdleState = new BossIdleState(this);
        AttackState = new BossAttackState(this);
        DieState = new BossDieState(this);
    }

    public void ChangeState(IBossState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public IBossState GetCurrentState()
    {
        return currentState;
    }
}