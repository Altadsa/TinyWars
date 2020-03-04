using UnityEngine;


/// <summary>
/// Base class for all items which must show data in the context menu
/// </summary>
[CreateAssetMenu(menuName = "Data/Menu Data")]
public class MenuData : ScriptableObject, IMenuItem
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [Range(0,15)]
    [SerializeField] private int _priority;
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Description => _description;
    public int Priority => _priority;
}    