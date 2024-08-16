using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SkillInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillDataSO data;
    public GameObject field;
    public TextMeshProUGUI textField;

    public void OnPointerEnter(PointerEventData eventData)
    {
        field.gameObject.SetActive(true);
        field.transform.position = transform.position;
        textField.text = data?.info;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        field.gameObject.SetActive(false);
    }
}
