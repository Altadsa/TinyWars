using UnityEngine;

[CreateAssetMenu(menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private BuildingType _buildingType;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;

    public BuildingType BuildingType => _buildingType;
    public string Name => _name;
    public Sprite Icon => _icon;

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
    STABLES,
    TOWER,
    TOWNHALL,
    WORKSHOP
}
