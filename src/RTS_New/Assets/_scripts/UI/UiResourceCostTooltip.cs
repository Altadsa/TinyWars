using TMPro;
using UnityEngine;

public class UiResourceCostTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _costText;

    public void SetActive(bool active, string textValue = "")
    {
        _costText.text = textValue;
        gameObject.SetActive(active);
    }
    
}