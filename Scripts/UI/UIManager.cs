using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class UIManager : Singleton<UIManager>
{
    public static float width = 1920f;
    public static float height = 1080f;

    Dictionary<string,UIBase> dict = new Dictionary<string,UIBase>();

    public T Open<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        dict.TryGetValue(uiName, out UIBase ui);

        if (ui == null)
        {
            Addressables.InstantiateAsync(uiName).Completed += (handle) =>
            {
                GameObject g = handle.Result;
                ui = g.GetComponent<UIBase>();

                ui.Show(uiName);
                dict[uiName] = ui;

                ui.canvas.sortingOrder = dict.Count;
                ui.Open(param);
            };
        }
        else ui.Open(param);

        return (T)ui;
        //commends.Push(ui);
    }

    public override void Awake()
    {
        Open<NewIntroUI>();
    }
}
