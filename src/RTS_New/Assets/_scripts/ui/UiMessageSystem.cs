using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMessageSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageText;
    
    private const string RESOURCE_REQUIREMENTS = "You are missing the following resources:\n";
    private const string BUILD_REQUIREMENTS = "You must meet the following Requirements to build:\n";
    private const string PLACEMENT_WARNING = "Invalid Building placement. Choose somewhere else.";

    private const float MESSAGE_DURATION = 5;

    public void BuildingPlacementMessage()
    {
        _messageText.text = PLACEMENT_WARNING;
        StartCoroutine(ShowMessage());
    }

    public void RequirementMessage(Player player, BuildingType[] requirements)
    {
        string reqMessage = "";
        foreach (var buildingType in requirements)
        {
            if (!player.RequirementsMet(requirements))
            {
                reqMessage += $"\t- {buildingType}\n";
            }
        }

        _messageText.text = BUILD_REQUIREMENTS + reqMessage;
        StartCoroutine(ShowMessage());
    }

    public void CostMessage(ResourceData data)
    {
        _messageText.text = RESOURCE_REQUIREMENTS;
        _messageText.text += data.ToString();
        StartCoroutine(ShowMessage());
    }
    
    IEnumerator ShowMessage()
    {
        _messageText.alpha = 1;
        _messageText.gameObject.SetActive(true);
        float elapsedTime = 0;
        while (elapsedTime < MESSAGE_DURATION)
        {
            if (elapsedTime > MESSAGE_DURATION / 2)
            {
                _messageText.alpha -= 0.01f * (MESSAGE_DURATION / 2);
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _messageText.text = "";
        _messageText.gameObject.SetActive(false);
    }
    
}
