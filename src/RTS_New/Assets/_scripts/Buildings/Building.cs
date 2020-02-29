using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Building : Entity
{
    [SerializeField] private BuildingData _buildingData;
    
    private bool _constructed = false;

    private void Awake()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        //Assuming Children have mesh renderers to set player material
        //TODO Change ref to complete model
        transform.GetChild(2).GetComponent<MeshRenderer>().material = Player.EntityMaterial;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
    }

    public void SetConstruction()
    {
        _constructed = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public Vector3 GetSize()
    {
        return GetComponentInChildren<BoxCollider>().size;
    }
    
    public override void Select()
    {
        Debug.Log("Selected " + name);
    }

    public override void Deselect()
    {
        Debug.Log("Deselected " + name);
    }
}
