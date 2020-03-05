using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    float Horizontal { get; }
    float Vertical { get; }

    void ReadInput();
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

public interface IUnitAction
{
    int Priority { get; }

    bool IsActionValid(RaycastHit actionTarget);
}


public enum Modifier
{
    Damage,
    Armour,
    Health,
    AttackSpeed,
    MoraleMax,
    MoraleDecay,
    MoveSpeed,
    GatherSpeed,
    BuildSpeed
}