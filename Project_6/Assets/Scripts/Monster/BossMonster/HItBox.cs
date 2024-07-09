using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float duration = 0.2f;
    public float curDuration = 0f;

    private void OnEnable()
    {
        curDuration = 0f;
    }

    private void Update()
    {
        curDuration += Time.deltaTime;
        if (curDuration >= duration)
        {
            gameObject.SetActive(false);
        }
    }
}