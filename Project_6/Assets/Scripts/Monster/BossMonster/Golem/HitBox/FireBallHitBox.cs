using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallHitBox : MonoBehaviour, IPunInstantiateMagicCallback // �Ϲ����� HitBox�� �ٸ��� ���� �� �ı��� �̷��������, �浹 �� �ı��Ǿ�� ��
{
    // ���� Ŀ������(3��)
    // ���� �ӵ��� ��ǥ�� ���ư�����
    // �÷��̾� ������ �ı�
    // �ٴ� ������ �ı� & ������
    // Ŀ���� ���߿� �÷��̾�� �浹 �� �ı� ��ž�ڷ�ƾ
    // �ϴ÷� ���ư� ��� 10�� �� ������Ʈ �����ؾ� ��
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
            Debug.Log($"{collision.gameObject.name}�� �ǰݵ�");
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
