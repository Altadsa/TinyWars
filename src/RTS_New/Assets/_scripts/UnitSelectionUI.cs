using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionUI : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    private GameObject[] _selectionObjects;

    // Start is called before the first frame update
    void Start()
    {
        SetObjects();
        _sc.SelectionUpdated += SelectionUpdated;
    }

    private void SelectionUpdated(List<Entity> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i] as Unit;
            
            _selectionObjects[i].GetComponentInChildren<Image>().sprite = unit.Data.Icon;
            _selectionObjects[i].SetActive(true);
           // _selectionObjects[i].GetComponent<Button>().onClick.AddListener( delegate {  });
        }

        for (int i = units.Count; i < _selectionObjects.Length; i++)
        {
            _selectionObjects[i].SetActive(false);
        }
    }

    private void SetObjects()
    {
        var grid = GetComponentInChildren<GridLayoutGroup>().transform;
        _selectionObjects = new GameObject[grid.childCount];
        for (int i = 0; i < grid.childCount; i++)
        {
            _selectionObjects[i] = grid.GetChild(i).gameObject;
            _selectionObjects[i].SetActive(false);
        }
    }
    
}
