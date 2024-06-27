using UnityEngine;

public class P_Dummy : MonoBehaviour
{
    public string dummyName;
    public float dummyHp;

    public void TakeDamage(float damage)
    {
        dummyHp -= damage;
        if(dummyHp <= 0)
        {
            Debug.Log($"{dummyName} ���");
        }
    }

    public bool IsDead()
    {
        if(dummyHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}