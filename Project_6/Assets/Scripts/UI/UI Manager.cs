using System.Collections;
using System.Collections.Generic;
//using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    public static int screenWidth = 1920;
    public static int screenHeight = 1080;

    private Dictionary<string, UIBase> UIList = new Dictionary<string, UIBase>();

    public T Show<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();

        UIList.TryGetValue(uiName, out UIBase ui);

        if(ui == null)
        {
            ui = Resources.Load<UIBase>($"UI/{uiName}");
            ui.Show(uiName);
            UIList.Add(uiName, ui);
        }

        ui.canvas.sortingOrder = UIList.Count;
        ui.Opened(param);

        return (T)ui;
    }


}
