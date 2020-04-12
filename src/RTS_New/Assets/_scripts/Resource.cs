using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ResourceType _resource;
    [SerializeField] private int _weight;
    [SerializeField] private int _resourceCount = 100;

    public ResourceType Type => _resource;
    public int Weight => _weight;
    
    public event Action OnResourceDepleted;
    
    public int Gather()
    {
        _resourceCount--;
        if (_resourceCount <= 0)
        {
            //TODO Implement despawn()
            ResourceDepleted();
        }

        return 1;
    }

    private void ResourceDepleted()
    {
        OnResourceDepleted?.Invoke();
        Destroy(gameObject, 1);
    }
    
    
}
