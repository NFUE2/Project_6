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

        // Ÿ�� ������Ʈ�� �߽ɿ� ��ġ�ϵ��� ����
        transform.position = target.GetComponent<Collider2D>().bounds.center;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            // Ÿ���� ���� �̵�
            transform.position = target.GetComponent<Collider2D>().bounds.center;
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
