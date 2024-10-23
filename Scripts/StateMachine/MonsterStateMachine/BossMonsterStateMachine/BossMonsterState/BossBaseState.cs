using UnityEngine;

public interface IBossState
{
    public void Enter();
    public void Exit();
}
public class BossBaseState : IBossState
{
    protected BossStateMachine stateMachine;
    public BossBaseState(BossStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }
}