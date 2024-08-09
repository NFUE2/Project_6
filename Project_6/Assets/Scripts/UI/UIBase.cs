using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    [HideInInspector]
    public Canvas canvas;

    public virtual void Show(string uiName)
    {
        var newCanvasObject = new GameObject(uiName + " Canvas");

        canvas = newCanvasObject.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasScaler.referenceResolution = new Vector2(UIManager.ScreenWidth, UIManager.ScreenHeight);
        newCanvasObject.gameObject.AddComponent<GraphicRaycaster>();

        var obj = Instantiate(gameObject, newCanvasObject.transform);
        obj.name = obj.name.Replace("(Clone)", "");
    }

    public virtual void Opened(params object[] param) { }

    protected virtual void Hide()
    {
        UIManager.Instance.Hide(gameObject.name);
    }
}
