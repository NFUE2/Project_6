using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    public GameObject target; // Ÿ���� ���
    private TargetSkill targetingSkill;
    public float rotationSpeed = 100f; // ȸ�� �ӵ� (�ʴ� ȸ�� ����)

    public void Initialize(GameObject target, TargetSkill skill)
    {
        this.target = target;
        this.targetingSkill = skill;
    }

    void Update()
    {
        if (target != null)
        {
            // Ÿ���� ���� �̵�
            transform.position = target.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }

        // ȸ�� ���� �߰�
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
