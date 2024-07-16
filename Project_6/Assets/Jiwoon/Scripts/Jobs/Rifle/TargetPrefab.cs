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
            transform.position = targetEnemy.transform.position; //이러면 타겟의 정중앙에 배치될듯
        }
    }

    //나중에 완성되면 다시 피드백
    private void OnMouseDown()
    {
        if (targetEnemy != null)
        {
            //player.TargetMarkerClicked(targetEnemy);
            Debug.Log($"타겟 마커 클릭: {targetEnemy.name}");
        }
        else
        {
            //player.TargetMarkerMissed();
            Debug.Log("타겟 마커 놓침 (적이 null)");
        }
        Destroy(gameObject);
    }


    //OnDisable 함수로 바꿔주세요
    private void OnDestroy()
    {
        Debug.Log($"타겟 마커 파괴됨: {targetEnemy?.name ?? "null"}");
    }
}
