using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingInfo
{
    private UiBuilding _uiBuilding;
    private PlayerSelectionController _sc;
    private GameObject _buildingInfo;
    private Building _target;
    
    public UiBuildingInfo(UiBuilding uiHandler, PlayerSelectionController sc, GameObject infoUi)
    {
        _uiBuilding = uiHandler;
        _sc = sc;
        _buildingInfo = infoUi;
        _sc.SelectionUpdated += SelectionUpdated;
        _buildingInfo.SetActive(false);
    }
    
    private void SelectionUpdated(List<Entity> entities)
    {
        ClearOldTarget();
        if (entities.Count == 0)
        {            
            _buildingInfo.SetActive(false);
            return;
        }
        if (entities[0] is Unit)
        {
            _buildingInfo.SetActive(false);
            return;
        }
        _buildingInfo.SetActive(true);
        _target = entities[0] as Building;
        SetupTarget();
    }

    private void ClearOldTarget()
    {
        if (!_target) return;
        ;
        _target.GetComponent<BuildingHealth>().HealthChanged -= UpdateBuildingHealth;
        var queue = _target.GetComponent<BuildingQueue>();
        if (queue)
        {
            queue.QueueProcessing -= ProcessQueue;
            queue.QueueChanged -= UpdateQueue;
        }
        _target = null;
    }
    
    private void SetupTarget()
    {
        var data = _target.BuildingData;
        _uiBuilding.Icon.sprite = data.Icon;
        
        // Set health info
        _target.Health.HealthChanged += UpdateBuildingHealth;
        var currentH = _target.Health.CurrentHealth;
        UpdateBuildingHealth(currentH, _target.GetModifierValue(Modifier.Health));
        
        // Set queue info
        ProcessQueue(0);
        var queue = _target.GetQueue();
        if (!queue) return;
        SetQueueButtons(queue, queue.Queue);
        queue.QueueChanged += UpdateQueue;
        queue.QueueProcessing += ProcessQueue;
    }

    private void UpdateBuildingHealth(float current, float max)
    {
        _uiBuilding.Health.fillAmount = (float)Math.Round(current / max, 2);
        _uiBuilding.HealthText.text = $"{current}/{max}";
    }

    private void UpdateQueue()
    {
        var queue = _target.GetQueue();
        if (!queue) return;
        var buildingQueue = queue.Queue;
        SetQueueButtons(queue,buildingQueue);
    }

    private void SetQueueButtons(BuildingQueue queue, List<Queueable> buildingQueue)
    {
        for (int i = 0; i < _uiBuilding.QueueButtons.Length; i++)
        {
            if (i < buildingQueue.Count)
            {
                var index = i;
                var itemIcon = buildingQueue[i].Icon;
                _uiBuilding.QueueButtons[i].onClick.RemoveAllListeners();
                _uiBuilding.QueueButtons[i].GetComponent<Image>().sprite = itemIcon; 
                _uiBuilding.QueueButtons[i].onClick.AddListener(delegate
                {
                    queue.RemoveFromQueue(index);
                });
                _uiBuilding.QueueButtons[i].gameObject.SetActive(true);
            }
            else
            {
                _uiBuilding.QueueButtons[i].gameObject.SetActive(false);
            }
        }
    }
    
    private void ProcessQueue(double progress)
    {
        _uiBuilding.ProgressBar.fillAmount = (float) progress;
    }
    
}