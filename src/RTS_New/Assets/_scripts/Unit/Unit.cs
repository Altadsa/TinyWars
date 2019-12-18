using System;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    ISelectionController selectionController;


    [SerializeField] UnitActions unitActions;
    //[SerializeField] UnitHealth health;
    [SerializeField] GameObject selectionIndicator;
    public Player Player { get; private set; }
    public Transform Transform => transform;

    public void Initialize(Player player)
    {
        Player = player;
        selectionIndicator.SetActive(false);
        FindObjectsOfType<PlayerController>()
            .FirstOrDefault(/*c => c.Player == Player*/)
            ?.SelectionController.Selectable.Add(this);
        //health.Initialize(player);
    }

    private void Start()
    {
        selectionIndicator.SetActive(false);
        FindObjectOfType<PlayerController>().SelectionController.Selectable.Add(this);
    }

    public void Select()
    {
        Debug.Log($"Selected by {Player}");
        selectionIndicator.SetActive(true);
    }

    public void Deselect()
    {
        Debug.Log($"Deselected by {Player}");
        selectionIndicator.SetActive(false);
    }

    public void AssignAction(RaycastHit actionTarget)
    {
        unitActions.DetermineAction(actionTarget);
    }
}