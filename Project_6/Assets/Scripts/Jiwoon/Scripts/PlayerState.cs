public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Jumping,
    Attacking,
    UsingSkill,
    Dead
}

public interface IPlayerAction
{
    void Execute();
}

public interface IAttack : IPlayerAction { }
public interface ISkillQ : IPlayerAction { }
public interface ISkillE : IPlayerAction { }
