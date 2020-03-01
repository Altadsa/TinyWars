using System.Collections.Generic;
using UnityEngine;

public class BuildingQueue : MonoBehaviour
{
    //Handles production of Queueable items relative to the building.
    //Unlike the generic queue we want to be able to view everything in the queue and remove items from
    //So we must implement our own custom queue

    private QueueRTS<Queueable> _buildQueue;

    private void Awake()
    {
        _buildQueue = new QueueRTS<Queueable>(5);
    }
    
    
}

public class QueueRTS<T>
{
    private readonly int _maxLength;
    private List<T> _queue;
    
    public QueueRTS(int maxLength)
    {
        _maxLength = maxLength;
        _queue = new List<T>();
    }

    public List<T> Queue => _queue;
    
    public T Dequeue()
    {
        var queueLen = _queue.Count;
        var item = _queue[queueLen - 1];
        _queue.Remove(item);
        return item;
    }

    public void Enequeue(T item)
    {
        var queueLen = _queue.Count;
        if (queueLen == _maxLength - 1)
        {
            Debug.LogWarning("Queue at max capacity.");
            return;
        }
        _queue.Insert(0, item);
    }
}