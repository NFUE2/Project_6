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
        players = GameManager.instance.players;//게임 매니저에서 가져오기
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
        foreach(GameObject p in players)
        {
            float distance = Vector2.Distance(p.transform.position, (Vector3)stateMachine.controller.offsetPos + stateMachine.controller.transform.position);

            if (distance < stateMachine.controller.data.searchDistance)
            {
                stateMachine.controller.target = p.transform;
                stateMachine.ChangeState(stateMachine.trackState);
            }
        }
    }


}
