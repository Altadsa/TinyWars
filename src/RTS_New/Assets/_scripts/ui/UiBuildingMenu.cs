using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingMenu
{
    private PlayerSelectionController _sc;
    private GameObject _buildingMenuGo;

    private Building _target;

    private Button[] Menubuttons;

    private BuildingRallyPoint _rallyPoint;
    
    public UiBuildingMenu(Button[] menuButtons, PlayerSelectionController sc, GameObject buildingMenuGo, 
        BuildingRallyPoint rallyPointPrefab)
    {
        Menubuttons = menuButtons;
        _sc = sc;
        _sc.SelectionUpdated += UpdateMenu;
        _buildingMenuGo = buildingMenuGo;
        _rallyPoint = Object.Instantiate(rallyPointPrefab);
        _rallyPoint.RallyPointSet();
    }

    private void UpdateMenu(List<Entity> entities)
    {
        if (entities.Count == 0 || entities[0] is Unit || entities[0] == null)
        {
            _rallyPoint.RallyPointSet();
            _buildingMenuGo.SetActive(false);
            return; 
        }
        _target = entities[0] as Building;
        
        _rallyPoint.SetRallyPoint(_target.RallyPoint);
        _buildingMenuGo.SetActive(true);
        
        var menuItems = _target.MenuItems;
        HideMenuButtons();
        
        if (menuItems == null) return;
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i] != null)
                SetButton(menuItems[i]);
        }

    }
    
    private void HideMenuButtons()
    {
        foreach (var menubutton in Menubuttons)
        {
            menubutton.gameObject.SetActive(false);
        }
    }

    private void SetButton(BuildingMenuItem item)
    {
        var i = item.Priority;
        Menubuttons[i].onClick.RemoveAllListeners();
        var qItem = item as Queueable;
        if (qItem)
        {
            Menubuttons[i].onClick.AddListener(delegate
            {
                _target.GetQueue().AddToQueue(qItem);
                if (qItem is BuildingUpgrade || qItem is ModifierUpgrade)
                {
                    Menubuttons[i].gameObject.SetActive(false);
                }
            });
        }
        else
        {
            Menubuttons[i].onClick.AddListener(delegate { item.Complete(_target); });            
        }


        Menubuttons[i].GetComponent<Image>().sprite = item.Icon;
        Menubuttons[i].gameObject.SetActive(true);
    }
    
}