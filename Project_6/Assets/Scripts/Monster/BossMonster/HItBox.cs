using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float originalSpeed;
    public MovableBoss mv;
    public float duration = 0.2f;
    public float curDuration = 0f;

    private void OnEnable()
    {
        originalSpeed = BossBattleManager.Instance.boss.moveSpeed;
        mv.speed = 0f;
        curDuration = 0f;
    }

    private void Update()
    {
        curDuration += Time.deltaTime;
        if (curDuration >= duration)
        {
            mv.speed = originalSpeed;
            gameObject.SetActive(false);
        }
    }
}