using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : Entity
{
    private bool _constructed = false;
    
    public override void Initialize(Player player)
    {
        base.Initialize(player);
        //Assuming Children have mesh renderers to set player material
        GetComponentInChildren<MeshRenderer>().material = Player.EntityMaterial;
    }

    public float GetHeight()
    {
        var size = GetComponentInChildren<BoxCollider>().size;
        return size.y;
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
