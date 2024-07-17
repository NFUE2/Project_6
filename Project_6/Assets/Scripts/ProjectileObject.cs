using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    public ProjectileDataSO data;

    private void Update()
    {
        transform.position += transform.right * data.moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerValue = data.target.value;
        int colLayer = collision.gameObject.layer;

        if(layerValue == 1 << colLayer && collision.TryGetComponent(out IDamagable player))
        {
            player.TakeDamage(data.damage);
            Destroy(gameObject);
        }
    }
}
