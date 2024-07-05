using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : IState

{
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {

    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }
    protected virtual void AddInputActionCallbacks()
    {
        
    }
    protected virtual void RemoveInputActionCallbacks()
    {
        
    }
    public virtual void HandleInput()
    {

    }
}
