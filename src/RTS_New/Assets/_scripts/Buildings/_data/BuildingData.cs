using UnityEngine;

[CreateAssetMenu(menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private BuildingType _buildingType;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    

    [Tooltip("How much the players max food capacity will increase")]
    [SerializeField] private int _foodProvided;
    
    public BuildingType BuildingType => _buildingType;
    public string Name => _name;
    public Sprite Icon => _icon;

    public int FoodProvided => _foodProvided;
}

public enum BuildingType
{
    ACADEMY,
    ARCHERY,
    BARRACKS,
    BLACKSMITH,
    CASTLE,
    CHAPEL,
    FARM,
    GRANARY,
    HOUSE,
    KEEP,
    LUMBERMILL,
    STABLES,
    TOWERA,
    TOWERB,
    TOWERC,
    TOWNHALL,
    WORKSHOP
}
