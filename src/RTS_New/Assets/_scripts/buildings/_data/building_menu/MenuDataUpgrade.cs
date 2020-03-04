using Units;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Menu Data (Upgrade)")]
public class MenuDataUpgrade : Queueable
{
    [SerializeField] private UnitType _entityKey;
    [SerializeField] private Modifier _modifierKey;
    [SerializeField] private float _value;

    public UnitType EntityKey => _entityKey;
    public Modifier ModifierKey => _modifierKey;
    public float Value => _value;

    public override void Complete(Building building)
    {
        var player = building.Player;
        player.ChangeModifier(_entityKey, _modifierKey, _value);
    }
}