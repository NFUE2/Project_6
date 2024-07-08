using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoss : MonoBehaviour
{
    private float speed;
    public float duration = 2.0f;
    private float elapsedTime = 0f;
    public bool movePositive = true;

    private void Start()
    {
        speed = BossBattleManager.Instance.boss.moveSpeed;
    }

    void Update()
    {
        var isAttacking = BossBattleManager.Instance.isAttacking;
        Debug.Log(isAttacking);
        if (isAttacking == false)
        {
            Vector3 targetPosition = BossBattleManager.Instance.targetPlayer.transform.position;
            float distance = Vector3.Distance(targetPosition, transform.position);
            if (distance > 2f)
            {
                
                if (targetPosition.x >= this.transform.position.x)
                {
                    movePositive = true;
                }
                else
                {
                    movePositive = false;
                }
                float direction = movePositive ? 1 : -1;
                transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
                //elapsedTime += Time.deltaTime;
            }
        }
    }
}
