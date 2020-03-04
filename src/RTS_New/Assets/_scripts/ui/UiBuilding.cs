using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    }

    private void UpdateMenu(List<Entity> entities)
    {
        if (entities.Count == 0)
        {
            _buildingMenuGo.SetActive(false);
            return; 
        }
        _target = entities[0] as Building;
        if (_target == null)
        {
            _buildingMenuGo.SetActive(false);
            return;
        }
        _buildingMenuGo.SetActive(true);
        var menuItems = _target.MenuItems;
        for (int i = 0; i < Menubuttons.Length; i++)
        {
            Menubuttons[i].gameObject.SetActive(false);
            if (i == 11)
            {
                Menubuttons[i].onClick.RemoveAllListeners();
                Menubuttons[i].GetComponent<Image>().sprite = SetRallyPointData.Icon;
                Menubuttons[i].onClick.AddListener(delegate { StartCoroutine(SetRallyPoint()); });
                Menubuttons[i].gameObject.SetActive(true);
            }
            else if (i == Menubuttons.Length - 1)
            {
                Menubuttons[i].onClick.RemoveAllListeners();
                Menubuttons[i].GetComponent<Image>().sprite = DestroyData.Icon;
                Menubuttons[i].onClick.AddListener(DestroySelectedBuilding);
                Menubuttons[i].gameObject.SetActive(true);
            }
            else if (menuItems != null)
            {
                if (i < menuItems.Length)
                {
                    var item = menuItems[i] as Queueable;
                    Menubuttons[i].onClick.RemoveAllListeners();
                    Menubuttons[i].onClick.AddListener(delegate { _target.GetQueue().AddToQueue(item); });
                    Menubuttons[i].GetComponent<Image>().sprite = item.Icon;
                    Menubuttons[i].gameObject.SetActive(true);
                }
            }
        }

    }
    
    private void DestroySelectedBuilding()
    {    
        Destroy(_target.gameObject);
        _target = null;
    }

    IEnumerator SetRallyPoint()
    {
        Vector3 newPosHit = Vector3.zero;
        while (!Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(1))
            {
                yield break;
            }
            newPosHit = _camera.Hit.Value.point;
            _rallyPoint.SetRallyPoint(newPosHit);
            yield return new WaitForEndOfFrame();
        }
        _target.SetRallyPoint(newPosHit);
    }

}