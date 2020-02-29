using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Player Player { get; private set; }
    
    public abstract void Select();

    public abstract void Deselect();

    //Used to initialise the entity after it is instantiated in the game world.
    public virtual void Initialize(Player player)
    {
        Player = player;
        //Add the Entity to the selection controller with same player
        FindObjectsOfType<PlayerController>()
            .FirstOrDefault(c => c.Player == Player)
            ?.SelectionController.Selectable.Add(this);
    }

    //To be used by entities to decided course of action on another entity/
    public bool IsAllied(Player player)
    {
        return player == Player;
    }
    
}
