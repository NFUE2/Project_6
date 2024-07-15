using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
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
        canvasScaler.referenceResolution = new Vector2(UIManager.screenWidth, UIManager.screenHeight);
        newCanvasObject.gameObject.AddComponent<GraphicRaycaster>();

        var obj = Instantiate(gameObject, newCanvasObject.transform);
        obj.name = obj.name.Replace("(Clone)", "");
    }

    public virtual void Opened(params object[] param) { }
}
