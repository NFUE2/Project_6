using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public float JumpForce { get; set; }
    public bool IsAttacking { get; set; }
    public int ComboIndex { get; set; }

    public Transform MainCamTransform { get; set; }
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunState RunState { get; }
    public PlayerJumpState JumpState { get; }

    public PlayerSkillQState SkillQState { get; }

    public PlayerSkillEState SkillEState { get; }

    public PlayerDieState DieState { get; }



    public PlayerStateMachine(Player player)
    {
        //생성자로 플레이어를 받음
        this.Player = player;
        MainCamTransform = Camera.main.transform;

        //상태를 전부 생성
        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);

        //데이터에서 받아옴
        //MovementSpeed = player.Data.GroundData.BaseSpeed;
        //RotationDamping = player.Data.GroundData.BaseRotationDamping;

    }
}
