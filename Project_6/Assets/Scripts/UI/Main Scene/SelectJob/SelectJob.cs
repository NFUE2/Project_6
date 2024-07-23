using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectJob : MonoBehaviour
{

    public Button SwordmanBTN;
    public Button ArcherBTN;
    public Button HammerBTN;
    public Button GunnerBTN;
    public Button SniperBTN;
    public Button PaladinBTN;
    public Button MagicianBTN;
    public Button AssassinBTN;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectJob_BTN()
    {
        if (SwordmanBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
        else if(ArcherBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
        else if (HammerBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
        else if (GunnerBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
        else if (SniperBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
        else if (PaladinBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
        else if (MagicianBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
        else if (AssassinBTN.GetComponent<Button_isOn>().ButtonIsOn == true)
        {

        }
    }
}
