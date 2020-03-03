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
    
    // We need to keep the elapsed time of the queue global in the event that we remove a processing item from the queue
    // and don't want to waste production time.
    private double _elapsedTime = 0;
    
    private void Awake()
    {
        _buildQueue = new QueueRts<Queueable>(5);
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
        _buildQueue.RemoveFromQueue(index);
        
        // Start the queue again if it isn't empty
        // Otherwise reset the elpased time
        if (!_buildQueue.IsEmpty())
            StartCoroutine(ProcessQueue(_elapsedTime));
        else
        {
            _elapsedTime = 0;
            QueueProcessing?.Invoke(_elapsedTime);
        }
        QueueChanged?.Invoke();
    }
    

    // We only want to update the Queue once it has items in it.
    IEnumerator ProcessQueue(double elapsedTime = 0)
    {
        do
        {
            _elapsedTime = elapsedTime;
            var item = _buildQueue.Peek();
            var queueTime = item.Time;
            while (_elapsedTime < queueTime)
            {
                _elapsedTime += Time.deltaTime;
                QueueProcessing?.Invoke(Math.Round(_elapsedTime/queueTime, 2));
                yield return new WaitForEndOfFrame();
            }
            QueueProcessing?.Invoke(0);
            item.Complete(GetComponent<Building>());
            
            _buildQueue.Dequeue();
            QueueChanged?.Invoke();
        } while (!_buildQueue.IsEmpty());
    }
    
}