using Units;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Menu Data (Upgrade)")]
public class ModifierUpgrade : Queueable
{
    [SerializeField] private UnitType _entityKey;
    [SerializeField] private Modifier _modifierKey;
    [SerializeField] private float _value;
    [SerializeField] private ModifierUpgrade _nextUpgrade;
    
    public UnitType EntityKey => _entityKey;
    public Modifier ModifierKey => _modifierKey;
    public float Value => _value;

    public override void Complete(Building building)
    {
        var player = building.Player;
        player.ChangeModifier(_entityKey, _modifierKey, _value);
        building.ReplaceItem(this, _nextUpgrade);
    }
}