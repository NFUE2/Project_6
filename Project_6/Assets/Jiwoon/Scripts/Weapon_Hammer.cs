using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hammer : MonoBehaviour
{
    private PlayerController_Hammer playerController;
    public float damage;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController_Hammer>();
    }

    public void EnableMeleeCollider()
    {
        if (playerController != null)
        {
            playerController.EnableMeleeCollider();
        }
    }
    public void DisableMeleeCollider()
    {
        if (playerController != null)
        {
            playerController.DisableMeleeCollider();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("적에게 데미지를 입혔습니다.");

        if (collision.TryGetComponent(out P_BossMonster boss))
            boss.TakeDamage(damage);

        if (playerController.meleeCollider != null)
        {
            playerController.meleeCollider.enabled = false;
        }
    }
}