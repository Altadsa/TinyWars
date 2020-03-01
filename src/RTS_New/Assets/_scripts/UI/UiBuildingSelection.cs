using System.Collections.Generic;
using UnityEngine;

public class UiBuildingSelection : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [SerializeField] private GameObject _selectionUI;
    // Start is called before the first frame update
    void Start()
    {
        _sc.SelectionUpdated += SelectionUpdated;
    }

    //The Building Selection UI needs to handle 2 things related to the selected building:
    // 1. Get the basic info of the building
    //        Icon
    //        Name
    //        Current Health
    //        Armour
    // 2. If the Building is constructed, then it must retrieve the queue information for that building.
    //        Item in production
    //        Items in queue
    //        Progress towards completion for the item

    private void SelectionUpdated(List<Entity> entities)
    {
        if (entities.Count == 0)
        {
            _selectionUI.SetActive(false);
            return;
        }
        if (entities[0] is Unit)
        {
            _selectionUI.SetActive(false);
            return;
        }
        _selectionUI.SetActive(true);
        
    }
    
}
