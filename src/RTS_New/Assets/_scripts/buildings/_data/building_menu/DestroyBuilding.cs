using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building Menu/Destroy Building")]
public class DestroyBuilding : BuildingMenuItem
{
    public override void Complete(Building building)
    {
        Destroy(building.gameObject);
    }
}