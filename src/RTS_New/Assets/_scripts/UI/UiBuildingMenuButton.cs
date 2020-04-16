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
    private static UiMessageSystem _messageSystem;
    
    private void Awake()
    {
        _menuButton = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();
        _messageSystem = FindObjectOfType<UiMessageSystem>();
    }

    public void SetButtonData(BuildingMenuItem item, Building building)
    {
        _data = item;
        _menuButton.onClick.RemoveAllListeners();
        if (item is Queueable queueable)
        {
            var player = building.Player;
            var isAvailable = player.RequirementsMet(queueable.Requirements);
            // If requirements aren't met, the we add the message to the button onclick.
            if (!isAvailable)
            {
                _menuButton.onClick.AddListener(delegate
                {
                    FindObjectOfType<UiMessageSystem>().RequirementMessage(player, queueable.Requirements);
                });
            }
            else 
            {
                var isUpgrade = queueable is BuildingUpgrade || queueable is ModifierUpgrade;
                var isUpgradeInQueue = false;
                if (isUpgrade)
                    isUpgradeInQueue = building.Queue.IsUpgradeInQueue(queueable); 
                if (isUpgradeInQueue) return;
                _menuButton.onClick.AddListener(delegate
                {
                    if (!player.CanAfford(queueable.Data))
                    {
                        var discrepancy = player.GetDiscrepancy(queueable.Data);
                        _messageSystem.CostMessage(discrepancy);
                    }
                    else
                    {
                        var addedToQueue = building.Queue.AddToQueue(queueable); 
                        if (addedToQueue)
                            player.DeductResources(queueable.Data);
                        
                        if (isUpgrade)
                        {
                            TooltipUpdated?.Invoke(_data, Input.mousePosition, false);
                            _menuButton.gameObject.SetActive(false);
                        }
                    }
                });
            }

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