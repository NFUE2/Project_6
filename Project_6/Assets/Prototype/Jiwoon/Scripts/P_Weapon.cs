using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Weapon : MonoBehaviour
{
    private PlayerController_Melee playerController;
    public float damage;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController_Melee>();
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

        if (collision.TryGetComponent(out P_Tentaclypse boss))
        {
            boss.TakeDamage(damage);
            Debug.Log("적에게 데미지를 입혔습니다.");
        }

        if (playerController.meleeCollider != null)
        {
            playerController.meleeCollider.enabled = false;
        }
    }
}