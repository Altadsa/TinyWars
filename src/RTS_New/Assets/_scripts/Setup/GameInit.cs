using UnityEngine;

public class GameInit : MonoBehaviour
{
    public int StartUnitCount = 1;
    public int PLayerCount = 1;
    public Unit StartUnit;
    public Building StartBuilding;
    public PlayerController PlayerPrefab;

    
    private Player[] _players;

    //public Dictionary<Color, Texture> entityTextures;
    public Material[] PlayerColors;
    
    private void Awake()
    {
        _players = new Player[PLayerCount];
        var realPlayerSet = false;
        for (int i = 0; i < PLayerCount; i++)
        {
            if (!realPlayerSet)
            {
                _players[i] = new Player(Color.blue, PlayerColors[i], false);
                var controller = Instantiate(PlayerPrefab);
                controller.Initialize(_players[i]);
            }
            else
            {
                
            }
            
        }

        foreach (var player in _players)
        {
            for (int i = 0; i < StartUnitCount; i++)
            {
                var newUnit = Instantiate(StartUnit);
                newUnit.Initialize(player);                
            }

            var newBuilding = Instantiate(StartBuilding);
            newBuilding.Initialize(player);
            newBuilding.GetComponentInChildren<MeshRenderer>().material = player.EntityMaterial;
        }
    }
    
    
}
