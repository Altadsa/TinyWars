using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building Upgrade")]
public class BuildingUpgrade : Queueable
{
    [SerializeField] private BuildingData _newData;
    [SerializeField] private GameObject _complete;
    public GameObject Completed => _complete;

    // If a building has more than 1 model upgrade, then place data in here.
    [SerializeField] private BuildingUpgrade _nextUpgrade;
    
    public override void Complete(Building building)
    {
        var player = building.Player;
        var oldModel = building.transform.GetChild(0);
        Destroy(oldModel.gameObject);
        var newModel = Instantiate(_complete, building.transform);
        newModel.GetComponent<MeshRenderer>().material = player.EntityMaterial;
    }
}