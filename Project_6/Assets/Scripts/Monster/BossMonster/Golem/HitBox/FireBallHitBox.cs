using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallHitBox : MonoBehaviour, IPunInstantiateMagicCallback // 일반적인 HitBox와 다르게 생성 및 파괴가 이루어져야함, 충돌 시 파괴되어야 함
{
    // 점점 커져야함(3초)
    // 일정 속도로 목표로 날아가야함
    // 플레이어 맞으면 파괴
    // 바닥 맞으면 파괴 & 불장판
    // 커지는 도중에 플레이어와 충돌 시 파괴 스탑코루틴
    // 하늘로 날아갈 경우 10초 후 오브젝트 삭제해야 함
    public GameObject target;
    public GameObject burningField;
    private float throwDuration = 10;
    private float curDuration;
    private float expandDuration = 3;
    private float speed = 10;
    private Vector3 startScale = new Vector3(0, 0, 1);
    private Vector3 endScale = new Vector3(1, 1, 1);
    private float startColliderRadius = 0;
    private float endColliderRadius = 1.5f;
    public CircleCollider2D circleCollider;
    private Vector3 targetPosition;
    GameObject targetObject;
    private Coroutine coroutine;
    private bool isThrown = false;

    public AudioClip audioClip;

    private void Start()
    {
        //targetObject = Instantiate(target, BossBattleManager.Instance.spawnedBoss.transform);
        //targetPosition = BossBattleManager.Instance.targetPlayer.transform.position;
        //targetObject.transform.position = targetPosition;

        if (!PhotonNetwork.IsMasterClient) return;

        Vector2 pos = BossBattleManager.Instance.targetPlayer.transform.position;
        targetObject = PhotonNetwork.Instantiate("Boss/" + target.name, pos, Quaternion.identity);
        //targetObject.transform.SetParent(BossBattleManager.instance.boss.transform);
        targetObject.transform.position = targetPosition;


        transform.localScale = startScale;
        coroutine = StartCoroutine(ScaleExpand());
    }

    private void Update()
    {
        if (isThrown)
        {
            curDuration += Time.deltaTime;
            if(curDuration >= throwDuration) 
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
        if(PhotonNetwork.IsMasterClient && BossBattleManager.Instance.bossStateMachine.GetCurrentState() == BossBattleManager.Instance.bossStateMachine.DieState)
        {
            PhotonNetwork.Destroy(gameObject); 
            //Destroy(gameObject);
        }
    }

    private IEnumerator ScaleExpand()
    {
        float currentTime = 0;
        while (currentTime <= expandDuration)
        {
            targetPosition = BossBattleManager.Instance.targetPlayer.transform.position;
            targetObject.transform.position = targetPosition;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / expandDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        coroutine = StartCoroutine(ThrowObject());
        transform.localScale = endScale;
    }

    private IEnumerator ThrowObject()
    {
        SoundManager.Instance.Shot(audioClip);
        isThrown = true;
        float currentTime = 0;
        //Destroy(targetObject);
        PhotonNetwork.Destroy(targetObject);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        while (true)
        {
            float distance = speed * Time.deltaTime;
            Vector3 newPosition = transform.position + moveDirection * distance;
           
            transform.position = newPosition;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name}가 피격됨");
            if (collision.TryGetComponent<IDamagable>(out IDamagable P) && collision.TryGetComponent<IKnockBackable>(out IKnockBackable K))
            {
                
                float damage = BossBattleManager.Instance.boss.attackPower * 1.25f;
                P.TakeDamage(damage);
                Vector2 playerPos = collision.transform.position;
                Vector2 bossPos = BossBattleManager.Instance.spawnedBoss.transform.position;

                Vector2 knockbackDirection = bossPos.x < playerPos.x ? new Vector2(1, 0) : new Vector2(-1, 0);
                K.ApplyKnockback(knockbackDirection, 5);
               
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            var field = Instantiate(burningField);
            field.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + 0.7f, collision.transform.position.z);
            Destroy(gameObject);
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.transform.SetParent(BossBattleManager.instance.boss.transform);
    }
}
