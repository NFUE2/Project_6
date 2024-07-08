using System.Collections;
using UnityEngine;

public class PistolAttack : MonoBehaviour, IAttack
{
   public void Execute()
    {
        Debug.Log("Pistol 스킬 Q 사용!");
    }
}
