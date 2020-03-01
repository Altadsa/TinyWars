using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private float _baseHealth;

    public string Name => _name;
    public Sprite Icon => _icon;
    public float BaseHealth => _baseHealth;

}
