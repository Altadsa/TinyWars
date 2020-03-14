using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Queue component for Buildings to produce items. 
/// </summary>
public class BuildingQueue : MonoBehaviour
{
    // Used to update the active items in the Queue.
    public event Action QueueChanged;
    // Used to update the production progress of the current item.
    public event Action<double> QueueProcessing;
    public List<Queueable> Queue => _buildQueue.Queue;

    private QueueRts<Queueable> _buildQueue;
    private Building _building;
    
    
    private void Awake()
    {
        _buildQueue = new QueueRts<Queueable>(5);
        _building = GetComponent<Building>();
    }

    private void OnEnable()
    {
        QueueChanged?.Invoke();
    }

    /// <summary>
    /// Add an item to the Building's Production Queue if there is space.
    /// </summary>
    /// <param name="item">The item to be added.</param>
    public void AddToQueue(Queueable item)
    {
        //If Queue is empty, we want to start processing it once we add an item.
        if (_buildQueue.IsEmpty())
        {
            _buildQueue.Enequeue(item);
            StartCoroutine(ProcessQueue());
        }
        else
        //Otherwise, it is already processing items, so just add another.
            _buildQueue.Enequeue(item);
        QueueChanged?.Invoke();
    }

    /// <summary>
    /// Remove an item from anywhere in the Queue.
    /// </summary>
    /// <param name="index">The index of the item to be removed.</param>
    public void RemoveFromQueue(int index)
    {
        // Halt Queue production while we remove the item
        StopAllCoroutines();
        
        var removedItem = _buildQueue.RemoveFromQueue(index);
        var player = _building.Player;
        
        player.RefundResources(removedItem.Data);
        
        // Start the queue again if it isn't empty
        // Otherwise reset the elpased time
        if (!_buildQueue.IsEmpty())
            StartCoroutine(ProcessQueue());
        else
            QueueProcessing?.Invoke(0);
        QueueChanged?.Invoke();
    }

    /// <summary>
    /// Checks for a Queueable item in the queue. To be used to check if upgrade items are in queue for ui.
    /// </summary>
    /// <param name="item">The item to check for membership.</param>
    /// <returns></returns>
    public bool IsUpgradeInQueue(Queueable item)
    {
        var queue = _buildQueue.Queue;
        return queue.Contains(item);
    }
    
    
    
    // We only want to update the Queue once it has items in it.
    IEnumerator ProcessQueue()
    {
        do
        {
            float elapsedTime = 0;
            var item = _buildQueue.Peek();
            var queueTime = item.Time;
            while (elapsedTime < queueTime)
            {
                elapsedTime += Time.deltaTime;
                QueueProcessing?.Invoke(Math.Round(elapsedTime/queueTime, 2));
                yield return new WaitForEndOfFrame();
            }
            item.Complete(_building);
            _buildQueue.Dequeue();
            QueueChanged?.Invoke();
        } while (!_buildQueue.IsEmpty());
        QueueProcessing?.Invoke(0);
    }
    
}