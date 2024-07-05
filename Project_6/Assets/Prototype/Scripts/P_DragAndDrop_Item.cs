using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class P_DragAndDrop_Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    /// <summary>
    /// ��������Ʈ ������: ��������Ʈ�� ���İ��� �����ϴµ� ���Ǵ� ���� ����.
    /// </summary>
    [SerializeField] Image image;

    /// <summary>
    /// �巡�װ� ���۵Ǵ� ������ X,Y ��ġ ���� ����.
    /// </summary>
    private float StartPositionX;
    private float StartPositionY;

    /// <summary>
    /// �ش� this ������Ʈ�� ���� ��ġ ���� ����.
    /// </summary>
    public Vector3 OriginPosition;

    /// <summary>
    /// �巡�׸� �ϴ��� ���ϴ��� üũ�ϱ� ���� bool ���� ����.
    /// </summary>
    private bool isDrag = false;

    /// <summary>
    /// �ش� ������Ʈ�� ���� ���� �������� ���Դ��� Ȯ���ϴ� bool ���� ����.
    /// </summary>
    public bool InBorder;

    /// <summary>
    /// �������� ����� �ϴ� ������Ʈ�� ��ġ ���� ����.
    /// </summary>
    private float DeletePositionX;
    private float DeletePositionY;




    void Start()
    {
        // [����] OriginPosition = this ������Ʈ�� ���� ��ġ �Է�.
        OriginPosition = this.transform.position;
    }




    public void OnDrag(PointerEventData eventData)
    {
        // [���ǹ�] Drag ����� �ϰ������� ���ǹ� ����.
        if (isDrag == true)
        {
            // [����] this ������Ʈ�� ��ġ = �巡�׸� ������ ���������� �Ÿ��� �Է�.
            (transform as RectTransform).anchoredPosition = new Vector2(eventData.position.x - StartPositionX, eventData.position.y - StartPositionY);
        }
    }




    /// <summary>
    /// �浹ü(Collider) ������ ���콺�� Ŭ���� �Ǿ����� �����ϴ� �Լ�.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // [���ǹ�] ���콺�� ��Ŭ�������� ���ǹ� ����.
        if (Input.GetMouseButtonDown(0))
        {
            // [����] �ش� ��������Ʈ�� ���İ��� 0.5�� ���߾� ������ ���·� �ٲپ���.
            image.color = new Color(1f, 1f, 1f, .5f);

            // [����] �巡�� ������ġ = ���콺 ���� ��ġ���� ������Ʈ�� ������ġ�� �� ������ �Է�.
            StartPositionX = eventData.position.x - (transform as RectTransform).anchoredPosition.x;
            StartPositionY = eventData.position.y - (transform as RectTransform).anchoredPosition.y;

            // [����] Drag ���¸� Drag ������·� ����
            isDrag = true;
        }
    }


    /// <summary>
    /// �浹ü(Collider) ������ ���콺�� Ŭ���� ���������� �����ϴ� �Լ�.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        // [����] �ش� ��������Ʈ�� ���İ��� 1�� ������ �������� ���·� �ٲپ���.
        image.color = new Color(1f, 1f, 1f, 1f);

        // [����] Drag ���¸� Drag �������·� ����
        isDrag = false;

        // [���ǹ�] ���� �������� ������ ��� ���ǹ� ����.
        if (InBorder == true)
        {
            // [����] ���� ���ӿ�����Ʈ�� ��ġ�� ���� ������ �������� ������ ��ġ�� �������� �Է�.
            gameObject.transform.localPosition = new Vector3(DeletePositionX, DeletePositionY, -1f);
        }
        // [���ǹ�] ���� �������� ������ �ʾ��� ��� ���ǹ� ����.
        else
        {   // [����] ���� ���� ������Ʈ�� ��ġ�� ���� ������Ʈ�� ��ġ�� �ǵ����� �Է�.
            gameObject.transform.position = OriginPosition;
        }

    }



    /// <summary>
    /// �浹�� �������� �� �����ϴ� �Լ�.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // [���ǹ�] ���� �浹ü�� �±װ� "DeleteSlot" �� ��� ���ǹ� ����.
        if (other.CompareTag("DeleteSlot"))
        {
            // [����] ���� ���� InBorder = ���� �������� �������� �ǹ��ϴ� true �� �Է�.
            InBorder = true;

            // [����] ���������� ��ġ�� �浹ü�� ��ġ�� �Է������ν� ���������� ��ġ�� ����.
            DeletePositionX = other.transform.position.x;
            DeletePositionY = other.transform.position.y;
        }
    }



    /// <summary>
    /// �浹�� �����ϰ� ���� �浹�� �������� ����Ǵ� �Լ�.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // [���ǹ�] ���� �浹ü�� �±װ� "DeleteSlot" �� ��� ���ǹ� ����.
        if (other.CompareTag("DeleteSlot"))
        {
            // [����] ���� ���� InBorder = ���� �ٱ����� �������� �ǹ��ϴ� false �� �Է�.
            InBorder = false;
        }
    }
}
