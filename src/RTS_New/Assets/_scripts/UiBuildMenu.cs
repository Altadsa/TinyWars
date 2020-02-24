using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildMenu : MonoBehaviour
{
    //When enabled, the build menu gets the available buildings for the player to purchase. Locked buildings should show their requirements and 
    //should be greyed out to indicate they are unavailable.
    
    //Menu needs a reference to the player to inspect its data
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Button[] _meunButtons;

    [Header("Debug")] public BuildMenuData[] _menuData;
    
    private void OnEnable()
    {
        //var buildings = _controller.Player;
        for (int i = 0; i < _meunButtons.Length; i++)
        {
            if (i < _menuData.Length)
            {
                _meunButtons[i].onClick.RemoveAllListeners();
                _meunButtons[i].GetComponent<Image>().sprite = _menuData[i].Icon;
                var building = _menuData[i].Building;
                _meunButtons[i].onClick.AddListener(delegate
                {
                    var newBuilding = Instantiate(building); 
                    newBuilding.Initialize(_controller.Player);
                });
                _meunButtons[i].gameObject.SetActive(true);
            }
            else
            {
                _meunButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
