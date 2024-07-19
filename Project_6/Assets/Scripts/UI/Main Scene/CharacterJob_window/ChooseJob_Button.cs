using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

// public void OnPointerClick(PointerEventData eventData)
// {
//     SwordmanBtn.GetComponent<Outline>().enabled = true;
//     ArcherBtn.GetComponent<Outline>().enabled = true;
//     HammerBtn.GetComponent<Outline>().enabled = true;
//     GunnerBtn.GetComponent<Outline>().enabled = true;
//     SniperBtn.GetComponent<Outline>().enabled = true;
//     PaladinBtn.GetComponent<Outline>().enabled = true;
//     MagicianBtn.GetComponent<Outline>().enabled = true;
//     AssassinBtn.GetComponent<Outline>().enabled = true;
// }
// 
// public void OnPointerEnter(PointerEventData eventData)
// {
//     SwordmanBtn.GetComponent<Outline>().enabled = true;
//     ArcherBtn.GetComponent<Outline>().enabled = true;
//     HammerBtn.GetComponent<Outline>().enabled = true;
//     GunnerBtn.GetComponent<Outline>().enabled = true;
//     SniperBtn.GetComponent<Outline>().enabled = true;
//     PaladinBtn.GetComponent<Outline>().enabled = true;
//     MagicianBtn.GetComponent<Outline>().enabled = true;
//     AssassinBtn.GetComponent<Outline>().enabled = true;
// }
// 
// public void OnPointerExit(PointerEventData eventData)
// {
//     SwordmanBtn.GetComponent<Outline>().enabled = false;
//     ArcherBtn.GetComponent<Outline>().enabled = false;
//     HammerBtn.GetComponent<Outline>().enabled = false;
//     GunnerBtn.GetComponent<Outline>().enabled = false;
//     SniperBtn.GetComponent<Outline>().enabled = false;
//     PaladinBtn.GetComponent<Outline>().enabled = false;
//     MagicianBtn.GetComponent<Outline>().enabled = false;
//     AssassinBtn.GetComponent<Outline>().enabled = false;
// }

    public void ColorChange(Button button)
    {
        //ColorBlock colorBlock = button.colors;

        //colorBlock.highlightedColor = new Color(0f, 1f, 0f, 1f);
        //colorBlock.pressedColor = new Color(0f, 0.5f, 0f, 1f);


        if (SwordmanBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            SwordmanBtn.GetComponent<Outline>().enabled = true;
        }
        else if (ArcherBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            ArcherBtn.GetComponent<Outline>().enabled = true;
        }
        else if (HammerBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            HammerBtn.GetComponent<Outline>().enabled = true;
        }
        else if (GunnerBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            GunnerBtn.GetComponent<Outline>().enabled = true;
        }
        else if (SniperBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            SniperBtn.GetComponent<Outline>().enabled = true;
        }
        else if (PaladinBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            PaladinBtn.GetComponent<Outline>().enabled = true;
        }
        else if (MagicianBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            MagicianBtn.GetComponent<Outline>().enabled = true;
        }
        else if (AssassinBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            //colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            //button.colors = colorBlock;

            AssassinBtn.GetComponent<Outline>().enabled = true;
        }
        else
        {
            //colorBlock.selectedColor = Color.white;
            //button.colors = colorBlock;

        }

        //button.colors = colorBlock;
    }
}
