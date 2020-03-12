using UnityEngine;

[CreateAssetMenu(menuName = "Data/BuildMenuData")]
public class BuildMenuData : MenuData, IResourceCost
{
    [SerializeField] private Building _buildingPrefab;
    
    [Tooltip("Gold Cost")]
    [SerializeField] private int _goldCost;
    [Tooltip("Lumber Cost")]
    [SerializeField] private int _lumberCost;
    [Tooltip("Iron Cost")]
    [SerializeField] private int _ironCost;
    [Tooltip("Food Cost")]
    [SerializeField] private int _foodCost;

    public Building Building => _buildingPrefab;
    public ResourceData Data => new ResourceData(_goldCost, 
        _lumberCost,
        _ironCost,
        _foodCost);
}
