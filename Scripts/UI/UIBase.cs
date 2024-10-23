using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public Canvas canvas;
    public void Show(string uiName)
    {
        GameObject c = new GameObject(uiName);
        transform.SetParent(c.transform);

        canvas = c.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler cs = c.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(UIManager.width, UIManager.height);

        c.AddComponent<GraphicRaycaster>();
    }

    public virtual void Open(params object[] param) => gameObject.SetActive(true);
    public virtual void Close(params object[] param) => gameObject.SetActive(false);
}