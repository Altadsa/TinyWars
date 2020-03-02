using Units;
using UnityEngine;

/// <summary>
/// Data object that stores initialisation and general information
/// </summary>
[CreateAssetMenu( menuName = "Data/UnitData")]
public class UnitData : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private UnitType _type;
    [SerializeField] private string _name;
    [SerializeField] private float _baseHealth;
    [SerializeField] private float _baseArmour;
    [SerializeField] private float _baseDamage;

    public Sprite Icon => _icon;
    public UnitType Type => _type;
    public string Name => _name;
    public float Health => _baseHealth;
    public float Armour => _baseArmour;
    public float Damage => _baseDamage;

}
