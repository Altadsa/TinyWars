using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Building : Entity
{
    private bool _constructed = false;

    private void Awake()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        GetComponentInChildren<BoxCollider>().enabled = false;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        //Assuming Children have mesh renderers to set player material
        GetComponentInChildren<MeshRenderer>().material = Player.EntityMaterial;
        GetComponentInChildren<BoxCollider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
    }

    public void SetConstruction(Vector3 constructionPosition)
    {
        _constructed = false;
        transform.GetChild(0).position = constructionPosition;
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
