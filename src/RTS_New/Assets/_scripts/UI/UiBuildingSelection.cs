﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingSelection : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [SerializeField] private GameObject _selectionUI;
    // Start is called before the first frame update
    
    [Header("Basic Building Info")] 
    public Image Icon;

    public Image Health;

    [Header("Queue Info")] 
    public Image ProgressBar;

    public Button[] QueueButtons;
    
    //Keep a reference to the target such that we can remove listeners from its events once the selection changes
    private Building _target;
    
    void Start()
    {
        _sc.SelectionUpdated += SelectionUpdated;
    }

    //The Building Selection UI needs to handle 2 things related to the selected building:
    // 1. Get the basic info of the building
    //        Icon
    //        Name
    //        Current Health
    //        Armour
    // 2. If the Building is constructed, then it must retrieve the queue information for that building.
    //        Item in production
    //        Items in queue
    //        Progress towards completion for the item

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
            queue.QueueProcessing += ProcessQueue;
        _target = null;
    }
    
    private void SetupTarget()
    {
        var data = _target.BuildingData;
        Icon.sprite = data.Icon;
        _target.GetComponent<BuildingHealth>().HealthChanged += UpdateBuildingHealth;
        
        var queue = _target.GetQueue();
        if (!queue) return;
        var buildingQueue = queue.Queue;
        for (int i = 0; i < QueueButtons.Length; i++)
        {
            if (i < buildingQueue.Count)
            {
                var itemIcon = buildingQueue[i].Icon;
                QueueButtons[i].GetComponent<Image>().sprite = itemIcon; 
                QueueButtons[i].onClick.AddListener(delegate
                {
                    queue.RemoveFromQueue(i);
                });
                QueueButtons[i].gameObject.SetActive(true);
            }
            else
            {
                QueueButtons[i].gameObject.SetActive(false);
            }
        }
        queue.QueueProcessing += ProcessQueue;
    }

    private void UpdateBuildingHealth(float current, float max)
    {
        Health.fillAmount = (float)Math.Round(current / max, 2);
    }

    private void ProcessQueue(double progress)
    {
        ProgressBar.fillAmount = (float) progress;
    }
    
}
