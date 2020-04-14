using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UiTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _requirements;

    [SerializeField] private GameObject _costGameObject;
    [SerializeField] private UiResourceCostTooltip[] _costTooltips;
    
    
    [SerializeField] private GameObject _tooltipGo;
    
    private const string REQ_TEXT = "Requirements:\n";

    private RectTransform _tooltipRect;

    private float _titleHeight = 0f,
        _descriptionHeight = 0f,
        _requirementsHeight = 0f,
        _costHeight = 0f;
    
    private void Awake()
    {
        FindObjectsOfType<UiBuildingMenuButton>().ToList().ForEach(b => b.TooltipUpdated += UpdateTooltip);
        FindObjectsOfType<UiBuildMenuButton>().ToList().ForEach(b => b.TooltipUpdated += UpdateTooltip);
        SetRectHeights();
        _tooltipGo.SetActive(false);
        _tooltipRect = _tooltipGo.GetComponent<RectTransform>();
    }

    private void UpdateTooltip(MenuData data, Vector3 newPos, bool setActive)
    {
        if (setActive == false)
        {
            _tooltipGo.SetActive(false);
            return;
        }
        _title.text = data.Name;
        _description.text = data.Description;
        _tooltipGo.SetActive(true);
        
        if (data is Queueable queueable)
        {
            LoadQueueableInfo(queueable);
        }
        else if (data is BuildMenuData building)
        {
            LoadBuildingInfo(building);
        }
        else
        {
            _requirements.gameObject.SetActive(false);
            _costGameObject.SetActive(false);
        }

        var newHeight = 0f;
        newHeight = 20 + _titleHeight + _descriptionHeight;
        if (_requirements.gameObject.activeInHierarchy)
            newHeight += _requirementsHeight;
        if (_costGameObject.activeInHierarchy)
            newHeight += _costHeight;
        _tooltipRect.sizeDelta = new Vector2(_tooltipRect.sizeDelta.x, newHeight);
        _tooltipRect.position = SetNetPosition(newPos);
    }

    private void LoadQueueableInfo(Queueable queueable)
    {
        LoadRequirements(queueable.Requirements);
        LoadItemCost(queueable.Data);
        _costTooltips[4].SetActive(true, queueable.Time.ToString());
    }

    private void LoadBuildingInfo(BuildMenuData data)
    {
        if (data.FoodProvided > 0)
            _description.text += $"\nProvides {data.FoodProvided} Food.";
        LoadRequirements(data.Requirements);
        LoadItemCost(data.Data);
        _costTooltips[4].SetActive(false);
    }

    private void LoadRequirements(BuildingType[] requirements)
    {
        if (requirements.Length > 0)
        {
            LoadRequirementData(requirements);
        }
        else
        {
            _requirements.gameObject.SetActive(false);
        }
    }

    private void LoadItemCost(ResourceData cost)
    {
        _costGameObject.SetActive(true);
        var resourceCosts = new int[] {cost.Gold, cost.Lumber, cost.Iron, cost.Food};
        for (int i = 0; i < resourceCosts.Length; i++)
        {
            var resourceCost = resourceCosts[i];
            if (resourceCost > 0)
            {
                _costTooltips[i].SetActive(true, resourceCosts[i].ToString());
            }
            else
            {
                _costTooltips[i].SetActive(false);
            }
        }
    }
    
    private void LoadRequirementData(BuildingType[] requirements)
    {
        _requirements.gameObject.SetActive(true);
        _requirements.text = REQ_TEXT;
        foreach (var req in requirements)
        {
            _requirements.text += $"\t-{req}";
        }   
    }
    
    private Vector3 SetNetPosition(Vector3 newPos)
    {
        var size = _tooltipRect.sizeDelta / 2;
        newPos.x -= size.x/2;
        newPos.y += size.y/2;
        return newPos;
    }

    private void SetRectHeights()
    {
        _titleHeight = _title.GetComponent<RectTransform>().sizeDelta.y;
        _descriptionHeight = _description.GetComponent<RectTransform>().sizeDelta.y;
        _requirementsHeight = _requirements.GetComponent<RectTransform>().sizeDelta.y;
        _costHeight = _costGameObject.GetComponent<RectTransform>().sizeDelta.y;
    }
    
}