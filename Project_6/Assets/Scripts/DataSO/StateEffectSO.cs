using UnityEngine;

[CreateAssetMenu(fileName = "StateEffectSO", menuName = "Scriptable Object/StateEffectSO")]

public class StateEffectSO : ObjectSO
{
    float intervalTime; //활성화 간격
    int count; //활성화 잔여횟수
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