using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] GameObject _hauptmenu; 
    [SerializeField] GameObject _tutorialMenu;

    private void Start()
    {
        _hauptmenu.SetActive(true);
        _tutorialMenu.SetActive(false);
    }
    public void SwitchMenu()
    {
        if(_hauptmenu.activeSelf == false)
        {
            _hauptmenu.SetActive(true);
            _tutorialMenu.SetActive(false);
        }
        else
        {
            _hauptmenu.SetActive(false);
            _tutorialMenu.SetActive(true);
        }
    }
}
