//using System.Collections;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class P_GunE : MonoBehaviour, P_ISkill
//{
//    //Animator animator;
//    Rigidbody2D rigidbody;
//    public float rollingX;
//
//    public float actionTime;
//    private float lastAction;
//    //public bool isRolling { get; private set; }
//    public bool isInvincible { get; private set; }
//
//    private void Awake()
//    {
//        //animator = GetComponentInChildren<Animator>();
//        rigidbody = GetComponent<Rigidbody2D>();
//        lastAction = -actionTime;
//    }
//
//    public void SkillAction()
//    {
//        if (Time.time - lastAction < actionTime) return;
//
//        //animator.SetTrigger("SkillE");
//        //if (isRolling) return;
//        //if (GetComponent<PlayerController_Gun>().isRolling) return;
//        //isRolling = true;
//        //GetComponent<PlayerController_Gun>().isRolling = true;
//
//        isInvincible = true;  // 구르기 시작 시 무적 상태로 설정
//
//        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
//        int c = dir.x > 0 ? 1 : -1;  
//
//        rigidbody.velocity = new Vector2(rollingX * c,0);
//
//        Invoke("ExitRolling",1.0f);
//        //SkillEAction?.Invoke();
//    }
//
//    public void ExitRolling()
//    {
//        //GetComponent<PlayerController_Gun>().isRolling = false;
//
//        isInvincible = false;  // 구르기 종료 시 무적 상태 해제
//
//        //isRolling = false;
//        rigidbody.velocity = Vector2.zero;
//        StartCoroutine(CoolTime());
//    }
//
//    IEnumerator CoolTime()
//    {
//        lastAction = Time.time;
//        //Text coolTimeText = GetComponent<PlayerController_Gun>().cooltimeEText;
//
//        while (Time.time - lastAction < actionTime)
//        {
//            //coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
//            yield return null;
//        }
//
//        //coolTimeText.text = "준비완료";
//    }
//}
//