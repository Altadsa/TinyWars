using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, ISelectable, IPlayerControllable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform Transform { get; }
    public void Select()
    {
        throw new System.NotImplementedException();
    }

    public void Deselect()
    {
        throw new System.NotImplementedException();
    }

    public Player Player { get; private set; }
    public void Initialize(Player player)
    {
        Player = player;
        GetComponentInChildren<MeshRenderer>().material = Player.EntityMaterial;
    }
}
