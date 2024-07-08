using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public IAttack attackComponent;
    public ISkillQ skillQComponent;
    public ISkillE skillEComponent;

    private void Start()
    {
        // �ν����Ϳ��� �Ҵ�� ������Ʈ�� �����ɴϴ�.
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        attackComponent.Execute();
    }

    public void OnSkillQ(InputAction.CallbackContext context)
    {
        skillQComponent.Execute();
    }

    public void OnSkillE(InputAction.CallbackContext context)
    {
        skillEComponent.Execute();
    }
}
