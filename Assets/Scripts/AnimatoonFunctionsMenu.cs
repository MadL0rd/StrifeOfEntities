using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatoonFunctionsMenu : MonoBehaviour
{
    private GameObject controller;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    void PanelHided()
    {
        controller.GetComponent<MenuController>().ShowCurrent();
    }
}
