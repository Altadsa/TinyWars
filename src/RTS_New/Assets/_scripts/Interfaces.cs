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
    List<ISelectable> Selectable { get; }
    List<ISelectable> Selected { get; }
}

public interface IPlayerController //: IPlayerControllable
{
    ISelectionController SelectionController { get; }
    IActionController ActionController { get; }
}

public interface IActionController
{
    void AssignUnitActions(List<ISelectable> selected, RaycastHit actionTarget);
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

public enum Modifier
{
    MeleeDamage,
    MeleeArmour,
    AttackSpeed,
    MoraleMax,
    MoraleDecay
}