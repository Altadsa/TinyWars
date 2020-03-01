using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingQueue : MonoBehaviour
{
    //Handles production of Queueable items relative to the building.
    //Unlike the generic queue we want to be able to view everything in the queue and remove items from
    //So we must implement our own custom queue
    public event Action QueueChanged;
    public event Action<double> QueueProcessing;
    
    private QueueRTS<Queueable> _buildQueue;
    private double _elapsedTime = 0;
    
    [Header("Debug Only")]
    public Queueable DebugItem;
    public int DebugCount;
    
    private void Awake()
    {
        _buildQueue = new QueueRTS<Queueable>(5);
        for (int i = 0; i < DebugCount; i++)
        {
            AddToQueue(DebugItem);
        }
    }

    public void AddToQueue(Queueable item)
    {
        //If Queue is empty, we want to start processing it once we add an item.
        if (_buildQueue.IsEmpty())
        {
            _buildQueue.Enequeue(item);
            StartCoroutine(ProcessQueue());
            return;
        }
        //Otherwise, it is already processing items, so just add another.
        _buildQueue.Enequeue(item);
    }

    public void RemoveFromQueue(int index)
    {
        Debug.LogFormat("Remove {0} from queue", index);
        StopAllCoroutines();
        _buildQueue.RemoveFromQueue(index);
        if (!_buildQueue.IsEmpty())
            StartCoroutine(ProcessQueue(_elapsedTime));
        QueueChanged?.Invoke();
    }
    
    public List<Queueable> Queue => _buildQueue.Queue;

    IEnumerator ProcessQueue(double elapsedTime = 0)
    {
        //Do this while there are elements in the queue
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
            item.Complete();
            _buildQueue.Dequeue();
            QueueChanged?.Invoke();
        } while (!_buildQueue.IsEmpty());
    }
    
}

public class QueueRTS<T>
{
    private readonly int _maxLength;
    private List<T> _queue;
    
    public QueueRTS(int maxLength)
    {
        _maxLength = maxLength;
        _queue = new List<T>(_maxLength);
    }

    public List<T> Queue => _queue;

    public bool IsEmpty()
    {
        return _queue.Count == 0;
    }

    public T Peek()
    {
        var queueLen = _queue.Count;
        return _queue[queueLen - 1];
    }
    
    public void Dequeue()
    {
        var queueLen = _queue.Count;
        var item = _queue[queueLen - 1];
        _queue.Remove(item);
    }

    public void Enequeue(T item)
    {
        var queueLen = _queue.Count;
        if (queueLen == _maxLength)
        {
            Debug.LogWarning("Queue at max capacity.");
            return;
        }
        _queue.Insert(0, item);
    }

    public void RemoveFromQueue(int index)
    {
        _queue.RemoveAt(index);
    }
}