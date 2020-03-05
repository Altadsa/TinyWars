using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class UiTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _requirements;

    [SerializeField] private GameObject _tooltipGo;
    
    private const string REQ_TEXT = "Requirements:\n";

    private void Awake()
    {
        FindObjectsOfType<UiBuildingMenuButton>().ToList().ForEach(b => b.TooltipUpdated += UpdateTooltip);
        _tooltipGo.SetActive(false);
    }

    private void UpdateTooltip(MenuData data, bool setActive)
    {
        Debug.Log("Update Tooltip");
        if (setActive == false)
        {
            _tooltipGo.SetActive(false);
            return;
        }
        _title.text = data.Name;
        _description.text = data.Description;
        _tooltipGo.SetActive(true);
    }
    
}
