using UnityEngine;
using UnityEngine.UI;

public class NPCBase : MonoBehaviour
{
    public float interactionDistance;

    public GameObject interactionUI; //������ ������ ������ �ȳ� UI
    public GameObject[] PopupUI; //��ȣ�ۿ� ������ ������ �� UI��

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        GetComponent<CircleCollider2D>().radius = interactionDistance;
    }

    public void Interaction()
    {
        foreach(GameObject g in PopupUI) g.SetActive(true);
    }

    void Active(bool active)
    {
        outline.enabled = active;
        interactionUI.SetActive(active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Active(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Active(false);
    }
}
