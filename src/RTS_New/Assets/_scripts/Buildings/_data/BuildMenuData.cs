using UnityEngine;

[CreateAssetMenu(menuName = "Data/BuildMenuData")]
public class BuildMenuData : MenuData
{
    [SerializeField] private Building _buildingPrefab;
    
    [Tooltip("Gold Cost")]
    [SerializeField] private int _goldCost;
    [Tooltip("Lumber Cost")]
    [SerializeField] private int _lumberCost;
    [Tooltip("Iron Cost")]
    [SerializeField] private int _ironCost;

    [SerializeField] private BuildingType[] _requirements;

    [Tooltip("How much the players max food capacity will increase")]
    [SerializeField] private int _foodProvided;
    
    /// <summary>
    /// Resource cost of the Building.
    /// </summary>
    public ResourceData Data => new ResourceData(_goldCost, 
        _lumberCost,
        _ironCost,
        0);
    
    public Building Building => _buildingPrefab;

    public BuildingType[] Requirements => _requirements;
    
    public int FoodProvided => _foodProvided;

}
