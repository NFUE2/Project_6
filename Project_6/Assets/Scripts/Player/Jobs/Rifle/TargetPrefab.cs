using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    private GameObject targetEnemy;
    private RiflePlayer player;
    private TargetSkill targetSkill;

    public void Initialize(GameObject enemy, RiflePlayer player, TargetSkill skill)
    {
        this.targetEnemy = enemy;
        this.player = player;
        this.targetSkill = skill;
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            transform.position = targetEnemy.transform.position;
        }
    }

    private void OnMouseDown()
    {
        if (targetEnemy != null)
        {
            targetSkill.TargetMarkerClicked(targetEnemy);
        }
        else
        {
            targetSkill.TargetMarkerMissed();
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log($"Å¸°Ù ¸¶Ä¿ ÆÄ±«µÊ: {targetEnemy?.name ?? "null"}");
    }
}
