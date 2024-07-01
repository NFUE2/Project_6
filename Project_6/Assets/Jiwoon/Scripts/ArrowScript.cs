using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Camera mainCamera;
    public float damage;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        CheckIfOffscreen();
    }

    void CheckIfOffscreen()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        // 화면 좌표를 이용하여 화면 밖에 있는지 확인
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject); // 화면 밖으로 나갔을 때 총알 파괴
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out P_Tentaclypse boss))
            boss.TakeDamage(damage);

        Destroy(gameObject);
        Debug.Log("적에게 데미지를 입혔습니다.");
    }
}
