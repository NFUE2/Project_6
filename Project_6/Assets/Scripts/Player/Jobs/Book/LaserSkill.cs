using UnityEngine;
using System.Collections;
using TMPro;

public class LaserSkill : SkillBase
{
    public float laserDuration = 2f;
    public float laserDamage = 10f;
    public LineRenderer laserRenderer;
    public LayerMask enemyLayer;
    public PlayerDataSO PlayerData;

    private void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;
        StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        float startTime = Time.time;
        laserRenderer.enabled = true;

        while (Time.time - startTime < laserDuration)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = (worldMousePosition - transform.position).normalized;

            Ray ray = new Ray(transform.position, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, enemyLayer);

            laserRenderer.SetPosition(0, transform.position);
            laserRenderer.SetPosition(1, transform.position + direction * 100);

            foreach (RaycastHit hit in hits)
            {
                // hit.collider.GetComponent<Enemy>().TakeDamage(laserDamage);
            }
            yield return null;
        }
        laserRenderer.enabled = false;
    }
}
