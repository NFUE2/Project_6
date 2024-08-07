using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    public static int ScreenWidth = 1920;
    public static int ScreenHeight = 1080;

    private Dictionary<string, UIBase> uiList = new Dictionary<string, UIBase>();

    public T Show<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        uiList.TryGetValue(uiName, out UIBase ui);
        if (ui == null)
        {
            Debug.Log($"UI/{uiName}");
            ui = Resources.Load<UIBase>($"UI/{uiName}");
            ui.Show(uiName);
            uiList.Add(uiName, ui);
        }

        ui.canvas.sortingOrder = uiList.Count;
        ui.Opened(param);

        return (T)ui;
    }

    public void Hide<T>()
    {
        string uiName = typeof(T).ToString();

        Hide(uiName);
    }

    public void Hide(string uiName)
    {
        uiList.TryGetValue(uiName, out UIBase ui);

        if (ui == null)
            return;

        DestroyImmediate(ui.canvas.gameObject);
        uiList.Remove(uiName);
    }
}
