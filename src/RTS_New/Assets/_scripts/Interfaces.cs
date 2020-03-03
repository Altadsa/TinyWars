using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    float Horizontal { get; }
    float Vertical { get; }

    void ReadInput();
}

public interface ISelectable
{
    Transform Transform { get; }
    void Select();
    void Deselect();
}

public interface ISelectionController
{
    List<Entity> Selectable { get; }
    List<Entity> Selected { get; }
}

public interface IPlayerController //: IPlayerControllable
{
    SelectionController SelectionController { get; }
    IActionController ActionController { get; }
}

public interface IActionController
{
    void AssignUnitActions(List<Entity> selected, RaycastHit actionTarget);
}

public interface IPlayerControllable
{
    Player Player { get; }

    void Initialize(Player player);
}

public interface IUpgradeable
{
    Dictionary<Modifier,float> Modifiers { get; }

    void UpdateModifiers();
}

public interface IUnitAction
{
    int Priority { get; }

    bool IsActionValid(RaycastHit actionTarget);
}

public interface IQueueable
{
    string Name { get; }
    string Description { get; }
    double QueueTime { get; }
    void Complete();
    //TODO Add Data Structure for costs

}

public enum Modifier
{
    Damage,
    Armour,
    AttackSpeed,
    MoraleMax,
    MoraleDecay
}