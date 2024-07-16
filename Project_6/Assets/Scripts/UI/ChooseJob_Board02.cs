using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class ChooseJob_Board02 : MonoBehaviour
{

    public TextMeshProUGUI Character_Name;
    public TextMeshProUGUI Character_Info;
    public Image Skill_Q;
    public Image Skill_E;
    public TextMeshProUGUI Q_Info;
    public TextMeshProUGUI E_Info;
    public Button Button;


    public void ChooseJob(ObjectSO objectSO, SkillDataSO Skill_Q, SkillDataSO Skill_E)
    {
        Character_Name.text = objectSO.name;
        Character_Info.text = objectSO.info;
        Q_Info.text = Skill_Q.info;
        E_Info.text = Skill_E.info;
        

        //Skill_Q = ;
        //Skill_E = ;
    }
}
