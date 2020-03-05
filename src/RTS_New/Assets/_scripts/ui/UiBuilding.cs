using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiBuilding : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [FormerlySerializedAs("_selectionUI")] 
    [SerializeField] private GameObject _buildingInfoGo;

    [SerializeField] private GameObject _buildingMenuGo;

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
    
    // References needed to visualise the rally point.
    [Header("Rally Point Data")]

    [SerializeField] private BuildingRallyPoint _rallyPoint;

    private UiBuildingInfo _buildingInfoUi;
    private UiBuildingMenu _buildingMenuUi;
    
    void Start()
    {
        _buildingInfoUi = new UiBuildingInfo(this, _sc, _buildingInfoGo);
        _buildingMenuUi = new UiBuildingMenu(Menubuttons, _sc, _buildingMenuGo, _rallyPoint);
    }
}