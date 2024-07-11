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
        if (targetEnemy != null)
        {
            player.TargetMarkerClicked(targetEnemy);
            Debug.Log($"Ÿ�� ��Ŀ Ŭ��: {targetEnemy.name}");
        }
        else
        {
            player.TargetMarkerMissed();
            Debug.Log("Ÿ�� ��Ŀ ��ħ (���� null)");
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log($"Ÿ�� ��Ŀ �ı���: {targetEnemy?.name ?? "null"}");
    }
}
