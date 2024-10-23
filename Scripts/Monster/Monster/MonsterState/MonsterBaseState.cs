using UnityEngine;

public class MonsterBaseState : IState
{
    protected MonsterStateMachine stateMachine;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput() { }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.controller.animator.SetBool(animatorHash, true);
    }
    protected void StartTriggerAnimation(int animatorHash)
    {
        stateMachine.controller.animator.SetTrigger(animatorHash);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.controller.animator.SetBool(animatorHash, false);
    }

    public float TargetDistance()
    {
        MonsterController controller = stateMachine.controller;
        Transform myTransform = controller.transform;

        Vector2 targetPos = controller.target.position;
        Vector2 offsetPos = controller.offsetPos;

        if (myTransform.localScale.x == -1)
            offsetPos = new Vector2(-offsetPos.x, offsetPos.y);

        Vector2 myPos = ((Vector2)myTransform.position + offsetPos);

        return Vector2.Distance(targetPos, myPos);
    }

    public bool IsTrackable()
    {
        MonsterController controller = stateMachine.controller;
        Transform myTransform = controller.transform;

        Vector2 targetPos = controller.target.position;
        Vector2 myPos = ((Vector2)myTransform.position + controller.offsetPos);

        Vector2 dir = targetPos - myPos;
        float distance = stateMachine.controller.data.attackDistance / 2;

        return !PlayerisDie() && Mathf.Abs(dir.x) >= distance;
    }

    public Vector2 TargetDirection()
    {
        Vector2 targetPos = stateMachine.controller.target.position;
        Vector2 myPos = stateMachine.controller.transform.position;

        return (targetPos - (myPos + stateMachine.controller.offsetPos)).normalized;
    }

    public void Aim()
    {
        stateMachine.controller.isRight = !(stateMachine.controller.data.isRight ^ TargetDirection().x > 0) ? true : false;
        stateMachine.controller.charcater.localScale =
        stateMachine.controller.isRight ? Vector3.one : new Vector3(-1, 1, 1);
    }

    public bool PlayerisDie()
    {
        stateMachine.controller.target.TryGetComponent(out PlayerInput p);

        return p.isDead;
    }

    protected bool GetNomalizeTime(string tag,float time)
    {
        Animator animator = stateMachine.controller.animator;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        bool check = animator.GetCurrentAnimatorStateInfo(0).IsTag(tag);

        if (check && info.normalizedTime >= 1.5f)
        {
            //StopAnimation(hash);
            return true;
        }
        return false;
    }
}