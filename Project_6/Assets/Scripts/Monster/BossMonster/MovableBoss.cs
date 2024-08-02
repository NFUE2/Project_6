using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoss : MonoBehaviourPun
{
    private float speed = 1f; //임시 고정값, 추후 수정
    public float duration = 2.0f;
    public bool movePositive = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
        //speed = BossBattleManager.Instance.boss.moveSpeed; //임시주석처리, 추후 수정
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

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
            if (distance > 3f)
            {
                
                if (targetPosition.x >= this.transform.position.x)
                {
                    movePositive = true;
                    photonView.RPC(nameof(FlipXRPC), RpcTarget.AllBuffered, false);
                    //spriteRenderer.flipX = false;
                }
                else
                {
                    movePositive = false;
                    photonView.RPC(nameof(FlipXRPC), RpcTarget.AllBuffered, true);
                    //spriteRenderer.flipX = true;
                }
                float direction = movePositive ? 1 : -1;
                transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
                //elapsedTime += Time.deltaTime;
            }
            else
            {
                animator.SetBool("isWalk", false);
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

    [PunRPC]
    void FlipXRPC(bool chk)
    {
        if(spriteRenderer != null) spriteRenderer.flipX = chk;
    }
}
