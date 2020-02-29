﻿
using System.Collections;
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
        Debug.Log("Building Selected");
        var worldPos = GetMouseLocation();
        var building = Instantiate(data.Building);
        var size = building.GetSize();
        //Set Build Area active and adjust size to that of selected building
        _buildArea.Hide(false);
        _buildArea.SetSize(size);
        while (!Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(building.gameObject);
                _buildArea.Hide(true);
                yield break;
            }
            worldPos = GetMouseLocation();
            _buildArea.transform.position = worldPos.Value;
            building.transform.position = worldPos.Value;
            yield return null;
        }
            
        if (!worldPos.HasValue)
        {
            Debug.LogError("No valid position selected.");
            yield break;
        }    

        building.transform.position = worldPos.Value - new Vector3(0, (size.y/2) + 1, 0);
        building.Initialize(_controller.Player);
        //SpawnBuilding(data.Building, worldPos.Value);
    }

    private bool IsPlacementValid(Building building)
    {
        return false;
    }
    
    private Vector3? GetMouseLocation()
    {
        Ray mouseRay = _mCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 100f, 9))
        {
            return SnapToPosition(hit.point);
        }

        return null;
    }
    
    private Vector3 SnapToPosition(Vector3 pos)
    {
        float pX = Mathf.Floor(pos.x);
        float pZ = Mathf.Floor(pos.z);
        return new Vector3(pX, pos.y, pZ);
    }

    private void SpawnBuilding(Building building, Vector3 spawnPos)
    {
        var newBuilding = Instantiate(building, spawnPos, Quaternion.identity); 
        newBuilding.Initialize(_controller.Player);
    }

}
