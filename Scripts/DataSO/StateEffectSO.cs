using UnityEngine;

[CreateAssetMenu(fileName = "StateEffectSO", menuName = "Scriptable Object/StateEffectSO")]

public class StateEffectSO : ObjectSO
{
    float intervalTime; //Ȱ��ȭ ����
    int count; //Ȱ��ȭ �ܿ�Ƚ��
}


public class StateEffectBase
{
    float intervalTime;
    int count;

    public StateEffectBase(float intervalTime, int count)
    {
        this.intervalTime = intervalTime;
        this.count = count;
    } 

    public void Enter()
    {

    }

    public void Exit()
    {

    }
}