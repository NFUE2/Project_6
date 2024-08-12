using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    public MonsterIdleState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    List<GameObject> players;

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.controller.animationData.idle);
        if(GameManager.instance != null) players = GameManager.instance.players;//게임 매니저에서 가져오기
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.controller.animationData.idle);
    }

    public override void HandleInput()
    {
        PlayerSearch();
    }

    private void PlayerSearch()
    {
        foreach(GameObject player in players)
        {
            float distance = Vector2.Distance(player.transform.position, (Vector3)stateMachine.controller.offsetPos + stateMachine.controller.transform.position);
            

            if (player.TryGetComponent(out PlayerInput p) && !p.isDead && distance < stateMachine.controller.data.searchDistance)
            {
                stateMachine.controller.target = player.transform;
                if(IsTrackable()) stateMachine.ChangeState(stateMachine.trackState);

                if (distance < stateMachine.controller.data.attackDistance)
                    stateMachine.ChangeState(stateMachine.attackState);
            }
        }
    }
}
