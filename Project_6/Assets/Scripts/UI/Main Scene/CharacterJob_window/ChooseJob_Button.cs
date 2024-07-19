using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseJob_Button : MonoBehaviour
{

    public Button SwordmanBtn;
    public Button ArcherBtn;
    public Button HammerBtn;
    public Button GunnerBtn;
    public Button SniperBtn;
    public Button PaladinBtn;
    public Button MagicianBtn;
    public Button AssassinBtn;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ColorChange(SwordmanBtn);
        ColorChange(ArcherBtn);
        ColorChange(HammerBtn);
        ColorChange(GunnerBtn);
        ColorChange(SniperBtn);
        ColorChange(PaladinBtn);
        ColorChange(MagicianBtn);
        ColorChange(AssassinBtn);
    }




    public void ColorChange(Button button)
    {
        ColorBlock colorBlock = button.colors;

        colorBlock.highlightedColor = new Color(0f, 1f, 0f, 1f);
        colorBlock.pressedColor = new Color(0f, 0.5f, 0f, 1f);


        if (SwordmanBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else if (ArcherBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else if (HammerBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else if (GunnerBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else if (SniperBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else if (PaladinBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else if (MagicianBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else if (AssassinBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            button.colors = colorBlock;
        }
        else
        {
            colorBlock.selectedColor = Color.white;
            button.colors = colorBlock;
        }

        button.colors = colorBlock;
    }
}
