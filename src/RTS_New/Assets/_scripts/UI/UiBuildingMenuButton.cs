using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles setup of menu buttons and tooltip data for a given menu item.
/// </summary>
public class UiBuildingMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BuildingMenuItem _data;
    private Button _menuButton;
    private Image _buttonImage;

    public event Action<BuildingMenuItem, Vector3,bool> TooltipUpdated;

    private static Camera _mainCamera;
    
    private void Awake()
    {
        _menuButton = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();
        
    }

    public void SetButtonData(BuildingMenuItem item, Building building)
    {
        _data = item;
        _menuButton.onClick.RemoveAllListeners();
        if (item is Queueable)
        {
            var isUpgrade = item is BuildingUpgrade || item is ModifierUpgrade;
            if (isUpgrade)
                isUpgrade =  building.GetQueue().IsUpgradeInQueue((Queueable) item);
            if (isUpgrade) return;
            _menuButton.onClick.AddListener(delegate
            {
                building.GetQueue().AddToQueue((Queueable) item);
                if (isUpgrade)
                {
                    _menuButton.gameObject.SetActive(false);
                }
            });
        }
        else
        {
            _menuButton.onClick.AddListener(delegate { item.Complete(building); });
        }

        _buttonImage.sprite = item.Icon;
        gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUpdated?.Invoke(_data, eventData.position, true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUpdated?.Invoke(_data, eventData.position,false);
    }

}
