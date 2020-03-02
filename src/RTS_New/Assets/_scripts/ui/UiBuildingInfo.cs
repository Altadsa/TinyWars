using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingInfo
{
    private UiBuilding _uiBuilding;
    private PlayerSelectionController _sc;
    private GameObject _selectionUI;
    private Building _target;
    
    public UiBuildingInfo(UiBuilding uiHandler, PlayerSelectionController sc, GameObject infoUi)
    {
        _uiBuilding = uiHandler;
        _sc = sc;
        _selectionUI = infoUi;
        _sc.SelectionUpdated += SelectionUpdated;
    }
    
    private void SelectionUpdated(List<Entity> entities)
    {
        ClearOldTarget();
        if (entities.Count == 0)
        {            
            _selectionUI.SetActive(false);
            return;
        }
        if (entities[0] is Unit)
        {
            _selectionUI.SetActive(false);
            return;
        }
        _selectionUI.SetActive(true);
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
        _target.GetComponent<BuildingHealth>().HealthChanged += UpdateBuildingHealth;
        
        var queue = _target.GetQueue();
        if (!queue) return;
        var buildingQueue = queue.Queue;
        for (int i = 0; i < _uiBuilding.QueueButtons.Length; i++)
        {
            if (i < buildingQueue.Count)
            {
                var itemIcon = buildingQueue[i].Icon;
                var index = i;
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
        queue.QueueChanged += UpdateQueue;
        queue.QueueProcessing += ProcessQueue;
    }

    private void UpdateBuildingHealth(float current, float max)
    {
        _uiBuilding.Health.fillAmount = (float)Math.Round(current / max, 2);
    }

    private void UpdateQueue()
    {
        var queue = _target.GetQueue();
        if (!queue) return;
        var buildingQueue = queue.Queue;
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