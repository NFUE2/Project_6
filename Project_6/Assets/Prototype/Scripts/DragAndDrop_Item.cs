using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop_Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    /// <summary>
    /// 스프라이트 렌더러: 스프라이트의 알파값을 조절하는데 사용되는 변수 선언.
    /// </summary>
    [SerializeField] Image image;

    /// <summary>
    /// 드래그가 시작되는 지점의 X,Y 위치 변수 선언.
    /// </summary>
    private float StartPositionX;
    private float StartPositionY;

    /// <summary>
    /// 해당 this 오브젝트의 원래 위치 변수 선언.
    /// </summary>
    public Vector3 OriginPosition;

    /// <summary>
    /// 드래그를 하는지 안하는지 체크하기 위한 bool 변수 선언.
    /// </summary>
    private bool isDrag = false;

    /// <summary>
    /// 해당 오브젝트가 일정 범위 안쪽으로 들어왔는지 확인하는 bool 변수 선언.
    /// </summary>
    public bool InBorder;

    /// <summary>
    /// 쓰레기통 기능을 하는 오브젝트의 위치 변수 선언.
    /// </summary>
    private float DeletePositionX;
    private float DeletePositionY;




    void Start()
    {
        // [변수] OriginPosition = this 오브젝트의 원래 위치 입력.
        OriginPosition = this.transform.position;
    }




    void Update()
    {
        // [조건문] Drag 기능을 하고있을때 조건문 실행.
        if (isDrag == true)
        {
            // [변수] mousePosition = 메인 카메라 중점을 기준으로 마우스의 위치값을 입력.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // [변수] this 오브젝트의 위치 = 드래그를 시작한 지점부터의 거리를 입력.
            this.gameObject.transform.position = new Vector2(mousePosition.x - StartPositionX, mousePosition.y - StartPositionY);
        }
    }




    /// <summary>
    /// 충돌체(Collider) 위에서 마우스가 클릭이 되었을때 실행하는 함수.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // [조건문] 마우스를 좌클릭했을때 조건문 실행.
        if (Input.GetMouseButtonDown(0))
        {
            // [변수] 해당 스프라이트의 알파값을 0.5로 낮추어 투명한 상태로 바꾸어줌.
            image.color = new Color(1f, 1f, 1f, 0.5f);

            // [변수] mousePosition = 메인 카메라 중점을 기준으로 마우스의 위치값을 입력.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // [변수] 드래그 시작위치 = 마우스 현재 위치에서 오브젝트의 원래위치를 뺀 값으로 입력.
            StartPositionX = mousePosition.x - this.transform.position.x;
            StartPositionY = mousePosition.y - this.transform.position.y;

            // [변수] Drag 상태를 Drag 실행상태로 변경
            isDrag = true;
        }
    }


    /// <summary>
    /// 충돌체(Collider) 위에서 마우스가 클릭을 해제했을때 실행하는 함수.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        // [변수] 해당 스프라이트의 알파값을 1로 높여서 불투명한 상태로 바꾸어줌.
        image.color = new Color(1f, 1f, 1f, 1f);

        // [변수] Drag 상태를 Drag 정지상태로 변경
        isDrag = false;

        // [조건문] 경계면 안쪽으로 들어왔을 경우 조건문 실행.
        if (InBorder == true)
        {
            // [변수] 현재 게임오브젝트의 위치를 경계면 안쪽의 쓰레기통 변수의 위치값 안쪽으로 입력.
            this.gameObject.transform.localPosition = new Vector3(DeletePositionX, DeletePositionY, -1f);
        }
        // [조건문] 경계면 안쪽으로 들어오지 않았을 경우 조건문 실행.
        else
        {   // [변수] 현재 게임 오브젝트의 위치를 원래 오브젝트의 위치로 되돌려서 입력.
            this.gameObject.transform.position = OriginPosition;
        }

    }



    /// <summary>
    /// 충돌을 감지했을 때 실행하는 함수.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // [조건문] 만일 충돌체의 태그가 "DeleteSlot" 일 경우 조건문 실행.
        if (other.CompareTag("DeleteSlot"))
        {
            // [변수] 경계면 변수 InBorder = 경계면 안쪽으로 들어왔음을 의미하는 true 값 입력.
            InBorder = true;

            // [변수] 쓰레기통의 위치를 충돌체의 위치로 입력함으로써 쓰레기통의 위치값 고정.
            DeletePositionX = other.transform.position.x;
            DeletePositionY = other.transform.position.y;
        }
    }



    /// <summary>
    /// 충돌을 감지하고 나서 충돌이 끝났을때 실행되는 함수.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // [조건문] 만일 충돌체의 태그가 "DeleteSlot" 일 경우 조건문 실행.
        if (other.CompareTag("Deleteslot"))
        {
            // [변수] 경계면 변수 InBorder = 경계면 바깥으로 나갔음을 의미하는 false 값 입력.
            InBorder = false;
        }
    }
}
