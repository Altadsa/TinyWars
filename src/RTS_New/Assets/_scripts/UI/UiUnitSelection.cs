using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiUnitSelection : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [SerializeField] private GameObject _selectionUI;
    [SerializeField] private Transform _selection;
    private GameObject[] _selectionObjects;

    [Header("****INFO PANEL****")] 
    public Image Icon;

    public Image Health;

    public TMP_Text HealthText;

    public TMP_Text DamageText;
    
    
    void Awake()
    {
        SetObjects();
        _sc.SelectionUpdated += SelectionUpdated;
        _selectionUI.SetActive(false);
    }

    private void SelectionUpdated(List<Entity> units)
    {
        if (units == null || units.Count == 0 || units[0] is Building)
        {
            _selectionUI.SetActive(false);
            return;
        }
        _selectionUI.SetActive(true);
        SetInfo(units[0] as Unit);
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i] as Unit;

            _selectionObjects[i].GetComponent<Image>().sprite = unit.Data.Icon;
            _selectionObjects[i].SetActive(true);
            var button = _selectionObjects[i].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                SetInfo(unit);
            });
        }

        for (int i = units.Count; i < _selectionObjects.Length; i++)
        {
            _selectionObjects[i].SetActive(false);
        }
    }

    private void SetInfo(Unit unit)
    {
        Icon.sprite = unit.Data.Icon;
        
    }
    
    private void SetObjects()
    {
        _selectionObjects = new GameObject[_selection.childCount];
        for (int i = 0; i < _selection.childCount; i++)
        {
            _selectionObjects[i] = _selection.GetChild(i).GetChild(0).gameObject;
            _selectionObjects[i].SetActive(false);
        }
    }

}