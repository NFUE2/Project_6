using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    private GameObject targetEnemy;
    private RiflePlayer player;

    public void Initialize(GameObject enemy, RiflePlayer player)
    {
        this.targetEnemy = enemy;
        this.player = player;
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
        player.TargetMarkerClicked(targetEnemy);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (targetEnemy == null)
        {
            player.TargetMarkerMissed();
        }
    }
}