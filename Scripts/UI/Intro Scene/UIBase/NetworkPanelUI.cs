using UnityEngine;
using TMPro;

public class NetworkPanelUI : UIBase
{
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject btn;

    private void OnEnable()
    {
        btn.SetActive(false);
    }

    public override void Open(params object[] param)
    {
        base.Open(param);

        message.text = (string)param[0];
        btn.SetActive((bool)param[1]);
    }
}