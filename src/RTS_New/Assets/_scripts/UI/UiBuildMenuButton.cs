using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiBuildMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<BuildMenuData, Vector3,bool> TooltipUpdated;

    private BuildMenuData _data;
    private Button _menuButton;
    private Image _buttonImage;
    private static Camera _mainCamera;
    private static UiMessageSystem _messageSystem;
    
    private void Awake()
    {
        _menuButton = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();
        _messageSystem = FindObjectOfType<UiMessageSystem>();
    }

    public void SetButtonData(BuildMenuData item, Player player)
    {
        _data = item;

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