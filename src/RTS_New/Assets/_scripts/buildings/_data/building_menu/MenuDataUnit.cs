using UnityEngine;

/// <summary>
/// Data object used for buildings to instantiate units.
/// </summary>
[CreateAssetMenu(menuName = "Data/Menu Data (Unit)")]
public class MenuDataUnit : Queueable
{
    [SerializeField] private Unit _unitPrefab;

    public override void Complete(Building building)
    {
        var spawnPos = building.UnitSpawn;
        var newUnit = Instantiate(_unitPrefab, spawnPos.position, Quaternion.identity);
        newUnit.Initialize(building.Player);
    }
}