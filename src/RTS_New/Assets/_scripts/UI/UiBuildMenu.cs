
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class UiBuildMenu : MonoBehaviour
{

    [SerializeField] private PlayerController _controller;
    [SerializeField] private BuildArea _buildArea;
    [FormerlySerializedAs("_meunButtons")] 
    [SerializeField] private UiBuildMenuButton[] _menuButtons;
    [SerializeField] private CameraController _cameraController;

    
    [Header("Debug")] public BuildMenuData[] _menuData;
    private Player _player;
    private Camera _mCam;

    private bool _buildingSelected = false;
    
    private void Awake()
    {
        _mCam = Camera.main;
    }

    private void OnEnable()
    {

        foreach (var menuButton in _menuButtons)
        {
            menuButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < _menuData.Length; i++)
        {
            var building = _menuData[i];
            var buttonIndex = building.Priority;
            _menuButtons[buttonIndex].SetButtonData(_menuData[i], _controller.Player);
            _menuButtons[buttonIndex].GetComponent<Image>().sprite = _menuData[i].Icon;

            var buildingCost = building.Data;
            var button = _menuButtons[buttonIndex].GetComponent<Button>();
            button.onClick.AddListener(delegate
            {
                StopAllCoroutines();
                var canAfford = _controller.Player.CanAfford(buildingCost);
                var requirementsMet = _controller.Player.RequirementsMet(building.Requirements);
                if (requirementsMet && canAfford)
                {
                    _controller.Player.DeductResources(buildingCost);
                    StartCoroutine(SelectBuilding(building));
                }
                else
                {
                    if (!canAfford)
                    {
                        var discrepancy = _controller.Player.GetDiscrepancy(buildingCost);
                        FindObjectOfType<UiMessageSystem>().CostMessage(discrepancy);
                    }
                    else
                        FindObjectOfType<UiMessageSystem>().RequirementMessage(_controller.Player, building.Requirements);
                }
            });
            _menuButtons[buttonIndex].gameObject.SetActive(true);
        }
    }

    private IEnumerator SelectBuilding(BuildMenuData data)
    {
        // Block camera controller from raycasting while we place the building.
        _cameraController.BlockRaycast(true);
        //World pos represents the position we want to place the building at.
        Vector3 worldPos = Vector3.zero;
        var building = Instantiate(data.Building);
        var size = building.Size;
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
                _cameraController.BlockRaycast(false);
                _controller.Player.RefundResources(data.Data);
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
                    FindObjectOfType<UiMessageSystem>().BuildingPlacementMessage();
                }
            }
            yield return null;
        }
        
        _buildArea.Hide(true);
        building.Initialize(_controller.Player);
        building.SetConstruction(false);
        // Restore camera controller raycasting ability.
        _cameraController.BlockRaycast(false);
    }

    private int _structuresLayer = 1 << 9;
    private bool IsPlacementValid(Building building)
    {
        var centre = building.transform.position;
        var size = building.Size;
        var tmp = size.y;
        size.y = size.z;
        size.z = tmp;
        //Get a list of overlapping colliders. We use the structures layer for selection
        var overlapCount = Physics.OverlapBox(centre, size / 2,
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
