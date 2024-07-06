using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterBaseState : MonoBehaviour, IState
{
    protected BossMonsterStateMachine stateMachine;
    public BossMonsterBaseState(BossMonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        // State 진입 시
    }

    public virtual void Exit()
    {
        // State 탈출 시
    }

    public virtual void HandleInput()
    {
        // 보스 몬스터는 사용 안함
    }

    // 보스 몬스터 제어 관여 메서드 추가 예정 필요에 따라 virtual 선언하여 override 사용
    // example
    // Move() 이동
    // SetTarget() 타겟 플레이어 설정
    // Rotate() 타겟 플레이어 방향으로 Sprite 회전
}
