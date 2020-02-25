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
                var building = _menuData[i];
                _meunButtons[i].onClick.AddListener(delegate
                {
                    //topAllCoroutines();
                    Debug.Log("Button Press");
                    StartCoroutine(SelectBuilding(building));
                });
                _meunButtons[i].gameObject.SetActive(true);
            }
            else
            {
                _meunButtons[i].gameObject.SetActive(false);
            }
        }
    }
    
    //Click on a button, we need to know which building data to pass
    //Wait for next left click such that we can spawn a building and deduct the appropriate resources

    private IEnumerator SelectBuilding(BuildMenuData data)
    {
        Debug.Log("Hello World");
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var worldPos = GetMouseLocation(mouseRay);
        Debug.Log("Spawn at " + worldPos);
        SpawnBuilding(data.Building, worldPos);
    }


    private Vector3 GetMouseLocation(Ray mouseRay)
    {
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            return hit.point;
        }
        return Vector3.one;
    }
    
    private void SpawnBuilding(Building building, Vector3 spawnPos)
    {
        var newBuilding = Instantiate(building, spawnPos, Quaternion.identity); 
        newBuilding.Initialize(_controller.Player);
    }
    

}
