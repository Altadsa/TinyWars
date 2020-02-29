
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
        Debug.Log("Building Selected");
        var worldPos = GetMouseLocation();
        var building = Instantiate(data.Building);
        var size = building.GetSize();
        //Set Build Area active and adjust size to that of selected building
        _buildArea.Hide(false);
        _buildArea.SetSize(size);
        while (!Input.GetMouseButtonDown(0))
        {
            var placementValid = IsPlacementValid(building);
            _buildArea.SetColor(placementValid);
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(building.gameObject);
                _buildArea.Hide(true);
                yield break;
            }

            if (worldPos.HasValue)
            {
                worldPos = GetMouseLocation();
                _buildArea.transform.position = worldPos.Value;
                building.transform.position = worldPos.Value;           
            }

            yield return null;
        }
            
        if (!worldPos.HasValue)
        {
            Debug.LogError("No valid position selected.");
            yield break;
        }
        _buildArea.Hide(true);
        var modelOffset = worldPos.Value - new Vector3(0, (size.y / 2) + 1, 0);
        building.SetConstruction(modelOffset);
        building.Initialize(_controller.Player);
    }

    private bool IsPlacementValid(Building building)
    {
        var collider = building.GetComponentInChildren<BoxCollider>();
        var centre = building.transform.position;
        var overlapCount = Physics.OverlapBox(centre, collider.size / 2,
            building.transform.rotation,
            1<<9);

        for (int i = 0; i < overlapCount.Length; i++)
        {
            Debug.LogFormat(this, "Collision: Name - {0} || ID: {1} ", overlapCount[i].gameObject.name, overlapCount[i].gameObject.GetInstanceID());
        }
        return overlapCount.Length == 0;
    }

    private int m = 1 << 10;
    private Vector3? GetMouseLocation()
    {
        Ray mouseRay = _mCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 100f, m))
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
    
}
