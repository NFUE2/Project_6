using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.Show<IntroPopup>();
    }
}
