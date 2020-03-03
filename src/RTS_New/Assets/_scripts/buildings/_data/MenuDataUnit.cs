using UnityEngine;

/// <summary>
/// Data object used for buildings to instantiate units.
/// </summary>
[CreateAssetMenu(fileName = "Data/Menu Data (Unit)")]
public class MenuDataUnit : Queueable
{
    [SerializeField] private Unit _unitPrefab;

    public override void Complete(Building building)
    {
        var spawnPos = building.transform.position - building.transform.forward * 5;
        var newUnit = Instantiate(_unitPrefab, spawnPos, Quaternion.identity);
        newUnit.Initialize(building.Player);
    }
}