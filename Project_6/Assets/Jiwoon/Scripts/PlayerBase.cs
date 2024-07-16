using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

    protected float lastQActionTime;  // Q 스킬 마지막 사용 시간
    protected float lastEActionTime;  // E 스킬 마지막 사용 시간
    protected float attackTime;  // 공격 시간 간격
    protected float lastAttackTime;  // 마지막 공격 시간

    // lastEActionTime에 접근하기 위한 메서드 추가
    public void SetLastEActionTime(float time)
    {
        lastEActionTime = time;
    }

    public float GetLastEActionTime()
    {
        return lastEActionTime;
    }
}
