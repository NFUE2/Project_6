using Photon.Pun;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    MonsterAttack attack;

    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine) 
    {
        switch(stateMachine.controller.type)
        {
            case MonsterAttackType.Single:
                attack = new MonsterSingleAttack(stateMachine);
                break;
            case MonsterAttackType.Multi:
                attack = new MonsterMultiAttack(stateMachine);
                break;
            case MonsterAttackType.Long:
                attack = new MonsterLongAttack(stateMachine);
                break;
        }
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.controller.animationData.attack);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.controller.animationData.attack);
    }

    public void Attack()
    {
        attack.Attack();
    }

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
