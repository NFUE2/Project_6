using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // �÷��̾��� �Է°� ���� �ӽ��� ������ �Ӽ���
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerController Input { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public PlayerData Data { get; private set; }

    private void Awake()
    {
        Input = new PlayerController();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>(); 
        Data = GetComponent<PlayerData>();

        StateMachine = new PlayerStateMachine(this);
    }

    private void OnEnable()
    {
        //Input.Enable();
    }

    private void OnDisable()
    {
        //Input.Disable();
    }

    private void Update()
    {
        //StateMachine.HandleInput();
        //StateMachine.Update();
    }

    private void FixedUpdate()
    {
        //StateMachine.PhysicsUpdate();
    }
}