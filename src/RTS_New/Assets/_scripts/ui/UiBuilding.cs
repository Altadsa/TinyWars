using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiBuilding : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [FormerlySerializedAs("_selectionUI")] 
    [SerializeField] private GameObject _buildingInfoGo;

    [SerializeField] private GameObject _buildingMenuGo;
    // Start is called before the first frame update
    [Header("****INFO PANEL****")]
    [Header("Basic Building Info")] 
    public Image Icon;

    public Image Health;

    [Header("Queue Info")] 
    public Image ProgressBar;
    public Button[] QueueButtons;
    
    // References needed to create the menu for buildings.
    [Header("****MENU****")]
    public Button[] Menubuttons;
    
    public MenuData DestroyData;
    public MenuData SetRallyPointData;

    // References needed to visualise the rally point.
    [Header("Rally Point Data")] 
    [SerializeField]
    private CameraController _camera;

    [SerializeField] private BuildingRallyPoint _rallyPoint;
    
    //Keep a reference to the target such that we can remove listeners from its events once the selection changes
    private Building _target;

    private UiBuildingInfo _buildingInfoUi;

    void Start()
    {
        _buildingInfoUi = new UiBuildingInfo(this, _sc, _buildingInfoGo);
        _sc.SelectionUpdated += UpdateMenu;
        _rallyPoint = Instantiate(_rallyPoint);
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
        
        //SetButton(DestroyData, DestroySelectedBuilding);
        //SetButton(SetRallyPointData, delegate { StartCoroutine(SetRallyPoint()); });
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
    
//    IEnumerator SetRallyPoint()
//    {
//        Vector3 newPosHit = Vector3.zero;
//        while (!Input.GetMouseButtonDown(0))
//        {
//            if (Input.GetMouseButtonDown(1))
//            {
//                _rallyPoint.RallyPointSet();
//                yield break;
//            }
//            newPosHit = _camera.Hit.Value.point;
//            _rallyPoint.SetRallyPoint(newPosHit);
//            yield return null;
//        }
//        _target.SetRallyPoint(newPosHit);
//    }

}