using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseJob_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Button SwordmanBtn;
    public Button ArcherBtn;
    public Button HammerBtn;
    public Button GunnerBtn;
    public Button SniperBtn;
    public Button PaladinBtn;
    public Button MagicianBtn;
    public Button AssassinBtn;

    public Image SwordmanIMG;
    public Image ArcherIMG;
    public Image HammerIMG;
    public Image GunnerIMG;
    public Image SniperIMG;
    public Image PaladinIMG;
    public Image MagicianIMG;
    public Image AssassinIMG;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    
    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void ColorChange(Button button)
    {

        SwordmanBtn.GetComponent<Button_Highlighted>().enabled = true;
        ArcherBtn.GetComponent<Button_Highlighted>().enabled = true;
        HammerBtn.GetComponent<Button_Highlighted>().enabled = true;
        GunnerBtn.GetComponent<Button_Highlighted>().enabled = true;
        SniperBtn.GetComponent<Button_Highlighted>().enabled = true;
        PaladinBtn.GetComponent<Button_Highlighted>().enabled = true;
        MagicianBtn.GetComponent<Button_Highlighted>().enabled = true;
        AssassinBtn.GetComponent<Button_Highlighted>().enabled = true;

        SwordmanBtn.GetComponent<Button_isOn>().ButtonIsOn = button == SwordmanBtn;
        ArcherBtn.GetComponent<Button_isOn>().ButtonIsOn = button == ArcherBtn;
        HammerBtn.GetComponent<Button_isOn>().ButtonIsOn = button == HammerBtn;
        GunnerBtn.GetComponent<Button_isOn>().ButtonIsOn = button == GunnerBtn;
        SniperBtn.GetComponent<Button_isOn>().ButtonIsOn = button == SniperBtn;
        PaladinBtn.GetComponent<Button_isOn>().ButtonIsOn = button == PaladinBtn; 
        MagicianBtn.GetComponent<Button_isOn>().ButtonIsOn = button == MagicianBtn;
        AssassinBtn.GetComponent<Button_isOn>().ButtonIsOn = button == AssassinBtn;

        
        SwordmanIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        ArcherIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        HammerIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        GunnerIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        SniperIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        PaladinIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        MagicianIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        AssassinIMG.color = new Color(0.5f, 0.5f, 0.5f, 1f);


        if (SwordmanBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            

            SwordmanBtn.GetComponent<Button_Highlighted>().enabled = false;
            
            
            SwordmanIMG.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (ArcherBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            
            ArcherBtn.GetComponent<Button_Highlighted>().enabled = false;

            
            ArcherIMG.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (HammerBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            

            HammerBtn.GetComponent<Button_Highlighted>().enabled = false;

            
            HammerIMG.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (GunnerBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            
            GunnerBtn.GetComponent<Button_Highlighted>().enabled = false;

            
            GunnerIMG.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (SniperBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            
            SniperBtn.GetComponent<Button_Highlighted>().enabled = false;

            
            SniperIMG.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (PaladinBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
            
            PaladinBtn.GetComponent<Button_Highlighted>().enabled = false;

            
            PaladinIMG.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (MagicianBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {
           
            MagicianBtn.GetComponent<Button_Highlighted>().enabled = false;

            
            MagicianIMG.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (AssassinBtn.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

            AssassinBtn.GetComponent<Button_Highlighted>().enabled = false;

            AssassinIMG.color = new Color(0f, 1f, 0f, 1f);
        }
    }
}
