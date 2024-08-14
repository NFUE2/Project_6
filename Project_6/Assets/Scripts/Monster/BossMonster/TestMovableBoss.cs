using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovableBoss : MonoBehaviour
{
    private float speed; //임시 고정값, 추후 수정
    public float duration = 2.0f;
    public bool movePositive = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
        speed = BossTestManager.Instance.boss.moveSpeed; //임시주석처리, 추후 수정
    }

    void Update()
    {
        var isAttacking = BossTestManager.Instance.isAttacking;
        //Debug.Log(isAttacking);
        if (isAttacking == false && BossTestManager.Instance.targetPlayer != null)
        {
            //if (animator.GetBool("isWalk") == false)
            //{
            //    animator.SetBool("isWalk", true);
            //}
            Vector3 targetPosition = BossTestManager.Instance.targetPlayer.transform.position;
            float distance = Vector3.Distance(targetPosition, transform.position);
            if (distance > 3f)
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
            //else
            //{
            //    animator.SetBool("isWalk", false);
            //}
        }
        else
        {
            //if (animator.GetBool("isWalk") == true)
            //{
            //    animator.SetBool("isWalk", false);
            //}
        }
    }
}
