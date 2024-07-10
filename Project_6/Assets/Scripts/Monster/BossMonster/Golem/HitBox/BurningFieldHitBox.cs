using UnityEngine;

public class BurningFieldHitBox : HitBox
{
    private float timeSinceLastTriggerStay = 0f;
    private float triggerStayInterval = 0.25f;

    private void OnEnable()
    {
        curDuration = 0;
        duration = 5f;
    }

    private void Update()
    {
        curDuration += Time.deltaTime;
        if (curDuration >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Time.time >= timeSinceLastTriggerStay + triggerStayInterval)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log($"{collision.gameObject.name}이 불장판에 의해 도트 데미지를 입고 있습니다.");
                timeSinceLastTriggerStay = Time.time;
            }
        }
    }
}