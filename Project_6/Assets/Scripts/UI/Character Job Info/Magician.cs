using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magician : MonoBehaviour
{
    public string JobName = "마법사";
    public string JobInfo = "마력이 담긴 마도서를 이용하여 적에게 마법공격하는 마법사.";
    public Image Skill_Q;
    public Image Skill_E;
    public string Q_Info = "자신의 주변에서 가장 가까운 적에게 데미지를 막아주는 실드를 걸어주는 기술.";
    public string E_Info = "자신이 지정한 방향으로 엄청난 마력을 모아서 마력의 광선을 방출하는 기술.";
}
