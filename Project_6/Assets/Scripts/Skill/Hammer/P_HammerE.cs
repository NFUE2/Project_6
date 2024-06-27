using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_HammerE : MonoBehaviour, P_ISkill
{
    private float damage;
    private float damageRate;
    private float maxChargingTime;
    private float attackDistance;

    private Animator animator;
    private bool isCharging;

    public void SkillAction()
    {
        isCharging = true;

        StartCoroutine(Charging());
    }

    IEnumerator Charging()
    {
        float startCharging = Time.time;

        while (!Input.GetKeyUp(KeyCode.E) && !(Time.time - startCharging < maxChargingTime))
        {
            damage += Time.deltaTime * damageRate;
            yield return null;
        }

        //Smash();
        //������ �ִ� �κ��� ��ũ��Ʈ�� ���� �ִϸ��̼� �̺�Ʈ�� ó������ ���
    }

    public void Smash()
    {
        //������ �Էºκ� �ʿ�
        Vector2 hitSize = new Vector2();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 dir = (sr.flipX ? Vector2.right : Vector2.left) * attackDistance; 

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir);

    }
}
