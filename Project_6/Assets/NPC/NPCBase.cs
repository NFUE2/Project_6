using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public abstract class NPCBase : MonoBehaviour
{
    public float distance;

    public GameObject interactionUI; //가까이 왔을때 나오는 안내 UI
    //public GameObject[] PopupUI; //상호작용 했을때 켜져야 할 UI들

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        Collider2D col  = GetComponent<Collider2D>();

        if (col is BoxCollider2D box) box.size = new Vector2(distance, distance);
        else if(col is CircleCollider2D circle) circle.radius = distance;
    }

    public abstract void Interaction();
    //foreach(GameObject g in PopupUI) g.SetActive(true);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) Interaction();
    }

    void Active(bool active)
    {
        outline.enabled = active;
        if(interactionUI != null) interactionUI?.SetActive(active);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Active(true);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        Active(false);
    }
}
