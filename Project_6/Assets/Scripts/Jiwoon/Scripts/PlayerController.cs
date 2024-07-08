using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public IAttack attackComponent;
    public ISkillQ skillQComponent;
    public ISkillE skillEComponent;

    private void Start()
    {
        // 인스펙터에서 할당된 컴포넌트를 가져옵니다.
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
