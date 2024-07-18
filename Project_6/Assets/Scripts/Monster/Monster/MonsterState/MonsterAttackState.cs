using System;
using System.Diagnostics;
using UnityEngine;
public class MonsterAttackState : MonsterBaseState
{
    //MonsterAttack attack;
    float lastAttackTime = -float.MaxValue;
    public bool isAttacking = false;

    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        //stateMachine.controller.GetComponent<MonsterAttack>().Initailize(this);
        //switch (stateMachine.controller.type)
        //{
        //    case MonsterAttackType.Single:
        //        attack = new MonsterSingleAttack(stateMachine);
        //        break;
        //    case MonsterAttackType.Multi:
        //        attack = new MonsterMultiAttack(stateMachine);
        //        break;
        //    case MonsterAttackType.Long:
        //        attack = new MonsterLongAttack(stateMachine);
        //        break;
        //}
        //attack = stateMachine.controller.GetComponent<MonsterAttack>();
    }

    //public override void Enter()
    //{
    //    base.Enter();
    //}

    //public override void Exit()
    //{
    //    base.Exit();
    //    //StopAnimation(stateMachine.controller.animationData.attack);
    //}

    public override void HandleInput()
    {
        base.HandleInput();


        if (!isAttacking && TargetDistance() > stateMachine.controller.data.attackDistance)
        {
            stateMachine.ChangeState(stateMachine.trackState);
        }

        if (Time.time - lastAttackTime >= stateMachine.controller.data.attackTime)
        {
            Aim();

            lastAttackTime = Time.time;
            StartTriggerAnimation(stateMachine.controller.animationData.attack);
            isAttacking = true;
            //StopAnimation(stateMachine.controller.animationData.attack);
            //attack.Attack();
        }

        if(isAttacking) GetNomalizeTime();
        //else
        //    lastAttackTime += Time.deltaTime;
        //멀어지면 다시 추적
        /*isAttacking = */
    }

    private void GetNomalizeTime()
    {
        Animator animator = stateMachine.controller.animator;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 1.0f) isAttacking = false;
    }


    //public void Attack() //애니메이션 이벤트용
    //{
    //    //attack.Attack();

    //    //Has Exit Time을 사용하면 애니메이션이 끝나고나서 넘어갈수있으니 괜찮을듯
    //    StopAnimation(stateMachine.controller.animationData.attack);
    //}

    //클래스 분리
    //public void Attack()
    //{
    //    MonsterType type = stateMachine.controller.type;

    //    if (type == MonsterType.ShotRange) CloseAttack();
    //    else LongAttack();
    //}

    //void CloseAttack()
    //{
    //    Vector2 myPos = stateMachine.controller.transform.position;
    //    int direction = stateMachine.controller.transform.localScale.x == 1 ? 1 : -1;
    //    LayerMask target = stateMachine.controller.targetLayer;
    //    //int attackRange = stateMachine.controller.data.att
    //    RaycastHit2D ray = Physics2D.Raycast(myPos, Vector2.right * direction, 5.0f,target);

    //    if(ray.transform.TryGetComponent(out IDamagable player))
    //        player.TakeDamage(stateMachine.controller.data.attackDamage);
    //}

    //void LongAttack()
    //{
    //    Vector2 myPos = stateMachine.controller.transform.position;
    //    Vector2 targetPos = stateMachine.controller.target.position;
    //    Vector2 direction = (targetPos - myPos).normalized;

    //    float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
    //    //string projectileName = stateMachine.controller.data. //투사체 이름
    //    //GameObject proejectile = PhotonNetwork.Instantiate(, myPos, Quaternion.identity); //복사할 투사체
    //    //proejectile.transform.localEulerAngles = new Vector3(0,0,angle); //투사체 각도
    //}
}
