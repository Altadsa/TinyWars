using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiUnitSelection : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [SerializeField] private GameObject _selectionUI;
    [SerializeField] private Transform _selection;
    private GameObject[] _selectionObjects;

    void Awake()
    {
        SetObjects();
        _sc.SelectionUpdated += SelectionUpdated;
    }

    private void SelectionUpdated(List<Entity> units)
    {
        if (!_selectionUI) return;
        if (units.Count == 0) 
        {
            _selectionUI.SetActive(false);
            return;
        }
        else if (units[0] is Building)
        {
            _selectionUI.SetActive(false);
            return;
        }
        _selectionUI.SetActive(true);
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
        _selectionObjects = new GameObject[_selection.childCount];
        for (int i = 0; i < _selection.childCount; i++)
        {
            _selectionObjects[i] = _selection.GetChild(i).gameObject;
            _selectionObjects[i].SetActive(false);
        }
    }

}
