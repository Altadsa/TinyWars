using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Player Player { get; private set; }

    protected SelectionController _pc;
    protected Dictionary<Modifier, float> _modifiers;
    
    //Used to initialise the entity after it is instantiated in the game world.
    public virtual void Initialize(Player player)
    {
        Player = player;
        //Add the Entity to the selection controller with same player
        _pc = FindObjectsOfType<PlayerController>()
            .FirstOrDefault(c => c.Player == Player)
            ?.SelectionController;
        _pc.AddSelected(this);
    }

    //To be used by entities to decided course of action on another entity/
    public bool IsAllied(Player player)
    {
        return player == Player;
    }

    protected void UpdateModifiers(Dictionary<Modifier, float> newModifiers)
    {
        _modifiers = newModifiers;
    }
    
}
