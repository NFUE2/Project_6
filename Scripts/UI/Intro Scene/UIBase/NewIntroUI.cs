using UnityEngine;
using UnityEngine.UI;

public class NewIntroUI : UIBase
{
    public static bool initalize = false;
    [SerializeField] private Button[] buttons = new Button[3];
    
    public override void Open(params object[] param)
    {
        base.Open(param);

        if(initalize)
        {
            initalize = true;
            UIManager.instance.Open<TutorialUI>();
        }
    }

    public void OnEnterNetwork()
    {
        NetworkManager.instance.Enter();
    }

    public void OnButton()
    {
        foreach(var b in buttons)
            b.interactable = false;
    }

    public void Initalize()
    {
        foreach (var b in buttons)
            b.interactable = false;
    }
}