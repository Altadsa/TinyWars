using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building Menu/Set Building Rally")]
public class SetBuildingRallyPoint : BuildingMenuItem
{
    public override void Complete(Building building)
    {
        building.SetRally();
    }
}