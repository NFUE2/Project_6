using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

    protected float lastQActionTime;  // Q ��ų ������ ��� �ð�
    protected float lastEActionTime;  // E ��ų ������ ��� �ð�
    protected float attackTime;  // ���� �ð� ����
    protected float lastAttackTime;  // ������ ���� �ð�

    // lastEActionTime�� �����ϱ� ���� �޼��� �߰�
    public void SetLastEActionTime(float time)
    {
        lastEActionTime = time;
    }

    public float GetLastEActionTime()
    {
        return lastEActionTime;
    }
}
