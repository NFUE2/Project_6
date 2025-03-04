using UnityEngine;
public class MonsterAttackState : MonsterBaseState 
{
    //MonsterAttack attack;
    float lastAttackTime = -float.MaxValue;
    public bool isAttacking = false;

    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void HandleInput()
    {
        base.HandleInput();

        if (!isAttacking && TargetDistance() > stateMachine.controller.data.attackDistance)
        {
            stateMachine.ChangeState(stateMachine.trackState);
        }

        if (!isAttacking && Time.time - lastAttackTime >= stateMachine.controller.data.attackTime)
        {
            Aim();
            StartAnimation(stateMachine.controller.animationData.attack);

            isAttacking = true;
            lastAttackTime = Time.time;
        }

        if(isAttacking && GetNomalizeTime("Attack", 1.5f))
        {
            isAttacking = false;
            StopAnimation(stateMachine.controller.animationData.attack);
        }

        if(PlayerisDie())
        {
            isAttacking = false;
            StopAnimation(stateMachine.controller.animationData.attack);
            stateMachine.controller.target = null;
            stateMachine.ChangeState(stateMachine.idleState);
        }
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
