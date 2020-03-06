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

    private RectTransform _tooltipRect;
    
    private void Awake()
    {
        FindObjectsOfType<UiBuildingMenuButton>().ToList().ForEach(b => b.TooltipUpdated += UpdateTooltip);
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
        if (data is Queueable queuable && queuable.Requirements.Length > 0)
        {
            _requirements.gameObject.SetActive(true);
            _requirements.text = REQ_TEXT;
            var reqs = queuable.Requirements;
            foreach (var req in reqs)
            {
                _requirements.text += $"\t-{req}";
            }
        }
        else
        {
            _requirements.gameObject.SetActive(false);
        }

        var newSize = _tooltipRect.sizeDelta;
        newSize.y = 20 + _title.GetComponent<RectTransform>().sizeDelta.y + _description.GetComponent<RectTransform>().sizeDelta.y;
        if (_requirements.gameObject.activeInHierarchy)
            newSize.y += _requirements.GetComponent<RectTransform>().sizeDelta.y + 5;
        _tooltipRect.sizeDelta = newSize;
        _tooltipRect.position = SetNetPosition(newPos);
    }
    
    private Vector3 SetNetPosition(Vector3 newPos)
    {
        var size = _tooltipRect.sizeDelta / 2;
        newPos.x -= size.x/2;
        newPos.y += size.y/2;
        return newPos;
    }
    
}
