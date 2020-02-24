using UnityEngine;

[CreateAssetMenu(menuName = "Data/BuildMenuData")]
public class BuildMenuData : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Building _buildingPrefab;

    public Sprite Icon => _icon;
    public Building Building => _buildingPrefab;
}
