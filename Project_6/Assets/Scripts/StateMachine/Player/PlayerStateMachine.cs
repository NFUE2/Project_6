using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunState RunState { get; }
    public PlayerJumpState JumpState { get; }
    public PlayerAttackState AttackState { get; }
    public PlayerDieState DieState { get; }

    public PlayerStateMachine(Player player)
    {
        //생성자로 플레이어를 받음
        this.Player = player;

        //상태를 전부 생성
        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        AttackState = new PlayerAttackState(this);
        DieState = new PlayerDieState(this);
    }
}


