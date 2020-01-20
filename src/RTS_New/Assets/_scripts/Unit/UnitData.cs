using Units;
using UnityEngine;

[CreateAssetMenu( menuName = "Data/UnitData")]
public class UnitData : ScriptableObject
{
    [SerializeField] private Texture _icon;
    [SerializeField] private UnitType _type;
    [SerializeField] private string _name;
    [SerializeField] private float _baseHealth;
    [SerializeField] private float _baseArmour;
    [SerializeField] private float _baseDamage;

    public Texture Icon => _icon;
    public UnitType Type => _type;
    public string Name => _name;
    public float Health => _baseHealth;
    public float Armour => _baseArmour;
    public float Damage => _baseDamage;

}
