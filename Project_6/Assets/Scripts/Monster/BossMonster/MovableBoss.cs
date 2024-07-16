using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoss : MonoBehaviour
{
    private float speed = 1f; //�ӽ� ������, ���� ����
    public float duration = 2.0f;
    public bool movePositive = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
        //speed = BossBattleManager.Instance.boss.moveSpeed; //�ӽ��ּ�ó��, ���� ����
    }

    void Update()
    {
        var isAttacking = BossBattleManager.Instance.isAttacking;
        Debug.Log(isAttacking);
        if (isAttacking == false && BossBattleManager.Instance.targetPlayer != null)
        {
            if(animator.GetBool("isWalk") == false)
            {
                animator.SetBool("isWalk", true);
            }
            Vector3 targetPosition = BossBattleManager.Instance.targetPlayer.transform.position;
            float distance = Vector3.Distance(targetPosition, transform.position);
            if (distance > 2f)
            {
                
                if (targetPosition.x >= this.transform.position.x)
                {
                    movePositive = true;
                    spriteRenderer.flipX = false;
                }
                else
                {
                    movePositive = false;
                    spriteRenderer.flipX = true;
                }
                float direction = movePositive ? 1 : -1;
                transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
                //elapsedTime += Time.deltaTime;
            }
        }
        else
        {
            if(animator.GetBool("isWalk") == true)
            {
                animator.SetBool("isWalk", false);
            }
        }
    }
}
