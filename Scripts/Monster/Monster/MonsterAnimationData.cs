using System;
using UnityEngine;

[Serializable]
public class MonsterAnimationData 
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string moveParameterName = "Move";
    [SerializeField] private string attackParameterName = "Attack";
    [SerializeField] private string dieParameterName = "Die";
    public int idle { get; private set; }
    public int move { get; private set; }
    public int attack { get; private set; }
    public int die { get; private set; }

    public void Initialize()
    {
        idle = Animator.StringToHash(idleParameterName);
        move = Animator.StringToHash(moveParameterName);
        attack = Animator.StringToHash(attackParameterName);
        die = Animator.StringToHash(dieParameterName);
    }
}
