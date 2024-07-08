using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "PistolClass", menuName = "Player/PistolClass")]
public class PistolClass : PlayerClass
{
    public GameObject attackPrefab;
    public GameObject skillQPrefab;
    public GameObject skillEPrefab;

    private void OnEnable()
    {
        className = "Pistol";
        attack = attackPrefab.GetComponent<IAttack>();
        //skillQ = skillQPrefab.GetComponent<ISkillQ>();
        //skillE = skillEPrefab.GetComponent<ISkillE>();
    }
}


public class PistolSkillQ : MonoBehaviour, ISkillQ
{
    public void Execute()
    {
        Debug.Log("Pistol ��ų Q ���!");
    }
}

public class PistolSkillE : MonoBehaviour, ISkillE
{
    public void Execute()
    {
        Debug.Log("Pistol ��ų E ���!");
    }
}
