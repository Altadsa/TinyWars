
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class UiBuildMenu : MonoBehaviour
{
    //When enabled, the build menu gets the available buildings for the player to purchase. Locked buildings should show their requirements and 
    //should be greyed out to indicate they are unavailable.
    
    //Menu needs a reference to the player to inspect its data
    [SerializeField] private PlayerController _controller;
    [SerializeField] private BuildArea _buildArea;
    [FormerlySerializedAs("_meunButtons")] 
    [SerializeField] private Button[] _menuButtons;

    [Header("Debug")] public BuildMenuData[] _menuData;

    private Building[] _placementModels;

    private Camera _mCam;
    
    private void Awake()
    {
        _mCam = Camera.main;
        _placementModels = new Building[_menuData.Length];
        for (int i = 0; i < _placementModels.Length; i++)
        {
            var building = _menuData[i].Building;
            var buildingInstance = Instantiate(building);
            _placementModels[i] = buildingInstance;
            _placementModels[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //var buildings = _controller.Player;
        for (int i = 0; i < _menuButtons.Length; i++)
        {
            //Setup buttons for which there exists build data
            if (i < _menuData.Length)
            {
                _menuButtons[i].onClick.RemoveAllListeners();
                _menuButtons[i].GetComponent<Image>().sprite = _menuData[i].Icon;
                var building = _menuData[i];
                _menuButtons[i].onClick.AddListener(delegate
                {
                    StopAllCoroutines();
                    StartCoroutine(SelectBuilding(building));
                });
                _menuButtons[i].gameObject.SetActive(true);
            }
            //set all remaining buttons to inactive
            else
            {
                _menuButtons[i].gameObject.SetActive(false);
            }
        }
    }
    
    //Click on a button, we need to know which building data to pass
    //Wait for next left click such that we can spawn a building and deduct the appropriate resources

    private IEnumerator SelectBuilding(BuildMenuData data)
    {
        //World pos represents the position we want to place the building at.
        Vector3 worldPos = Vector3.zero;
        var building = Instantiate(data.Building);
        var size = building.GetSize();
        //Set Build Area active and adjust size to that of selected building
        _buildArea.Hide(false);
        _buildArea.SetSize(size);
        bool canPlace = false;
        while (!canPlace)
        {
            var placementValid = IsPlacementValid(building);
            _buildArea.SetColor(placementValid);
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(building.gameObject);
                _buildArea.Hide(true);
                yield break;
            }
            
            worldPos = GetMouseLocation(worldPos);
            _buildArea.transform.position = worldPos;
            building.transform.position = worldPos;

            if (Input.GetMouseButtonDown(0))
            {
                if (placementValid)
                    canPlace = true;
                else
                {
                    Debug.LogWarning("Cant place building due to obstruction!", this);
                }
            }
            yield return null;
        }
        
        _buildArea.Hide(true);
        building.SetConstruction();
        building.Initialize(_controller.Player);
    }

    private int _structuresLayer = 1 << 9;
    private bool IsPlacementValid(Building building)
    {
        var collider = building.GetComponent<BoxCollider>();
        var centre = building.transform.position;
        //Get a list of overlapping colliders. We use the structures layer for selection
        var overlapCount = Physics.OverlapBox(centre + collider.center, collider.size / 2,
            building.transform.rotation,
            _structuresLayer);
        for (int i = 0; i < overlapCount.Length; i++)
        {
            Debug.LogFormat(this, "Collision: Name - {0} || ID: {1} ", overlapCount[i].gameObject.name, overlapCount[i].gameObject.GetInstanceID());
        }
        return overlapCount.Length == 0;
    }

    private int _terrainLayer = 1 << 10;
    private Vector3 GetMouseLocation(Vector3 oldPosition)
    {
        Ray mouseRay = _mCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //If we successfully hit an object, then return the new hit point
        if (Physics.Raycast(mouseRay, out hit, 100f, _terrainLayer))
        {
            return SnapToPosition(hit.point);
        }
        //otherwise, return the input (old) hit point.
        return oldPosition;
    }
    
    private Vector3 SnapToPosition(Vector3 pos)
    {
        float pX = Mathf.Floor(pos.x);
        float pZ = Mathf.Floor(pos.z);
        return new Vector3(pX, pos.y, pZ);
    }
    
}
