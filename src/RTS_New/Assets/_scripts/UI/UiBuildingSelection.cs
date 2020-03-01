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
