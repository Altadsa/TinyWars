using UnityEngine;

/// <summary>
/// Base class for all items which must show data in the entity menu
/// </summary>
[CreateAssetMenu(menuName = "Data/Menu Data")]
public class MenuData : ScriptableObject
{
    [Tooltip("Icon to represent the item in the menu.")]
    [SerializeField] private Sprite _icon;
    
    [Tooltip("Name of the item as it will appear in the menu.")]
    [SerializeField] private string _name;
    
    [Tooltip("Description of the item as it will appear in the menu.")]
    [SerializeField] private string _description;
    
    [Tooltip("The index of the item i.e. where it will appear in the menu.")]
    [Range(0,15)]
    [SerializeField] private int _priority;
    
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Description => _description;
    public int Priority => _priority;
}    