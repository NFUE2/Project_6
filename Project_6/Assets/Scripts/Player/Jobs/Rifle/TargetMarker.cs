using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    public GameObject target; // 타겟팅 대상
    private TargetSkill targetingSkill;
    public float rotationSpeed = 100f; // 회전 속도 (초당 회전 각도)

    public void Initialize(GameObject target, TargetSkill skill)
    {
        this.target = target;
        this.targetingSkill = skill;
    }

    void Update()
    {
        if (target != null)
        {
            // 타겟을 따라 이동
            transform.position = target.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }

        // 회전 로직 추가
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        if (target != null)
        {
            targetingSkill.OnTargetClicked(target);
            Destroy(gameObject);
        }
    }
}
