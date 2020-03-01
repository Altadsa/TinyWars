using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BuildingHealth))]
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
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
    }

    public void SetConstruction()
    {
        gameObject.AddComponent<BuildingConstruction>();
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
        if (GetComponent<BuildingConstruction>())
            Debug.LogFormat(this, "Building not fully constructed.");
    }

    public override void Deselect()
    {
        Debug.Log("Deselected " + name);
    }
}
