using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A List that uses Queue functionality with the added option to remove from anywhere in the Queue.
/// </summary>
/// <typeparam name="T">The Type of the Queue Items.</typeparam>
public class QueueRts<T>
{
    private readonly int _maxLength;
    private List<T> _queue;
    
    public QueueRts(int maxLength)
    {
        _maxLength = maxLength;
        _queue = new List<T>(_maxLength);
    }

    public List<T> Queue => _queue;

    /// <summary>
    /// Checks if the Queue contains any items.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return _queue.Count == 0;
    }

    /// <summary>
    /// Get the first item in the Queue without removing it.
    /// </summary>
    /// <returns></returns>
    public T Peek()
    {
        var queueLen = _queue.Count;
        return _queue[queueLen - 1];
    }
    
    /// <summary>
    /// Remove the first item from the Queue
    /// </summary>
    public void Dequeue()
    {
        var item = _queue[0];
        _queue.Remove(item);
    }

    /// <summary>
    /// Add an item to the Queue, if there is space.
    /// </summary>
    /// <param name="item">The item to be added.</param>
    public void Enequeue(T item)
    {
        var queueLen = _queue.Count;
        if (queueLen == _maxLength)
        {
            Debug.LogWarning("Queue at max capacity.");
            return;
        }
        _queue.Insert(queueLen, item);
    }

    /// <summary>
    /// Special function to remove any item from the Queue.
    /// </summary>
    /// <param name="index">The index of the item to be removed.</param>
    public void RemoveFromQueue(int index)
    {
        _queue.RemoveAt(index);
    }
}