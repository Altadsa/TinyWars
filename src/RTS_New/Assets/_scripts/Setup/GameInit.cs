using UnityEngine;

public class GameInit : MonoBehaviour
{
    public int StartUnitCount = 1;
    public int PLayerCount = 1;
    public Unit StartUnit;
    public Building StartBuilding;
    public PlayerController PlayerPrefab;
    public PlayerController AiPrefab;
    
    public Transform[] StartPositions;
    
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
                var controller = Instantiate(PlayerPrefab, StartPositions[i].position, Quaternion.identity);
                controller.Initialize(_players[i]);
                realPlayerSet = true;
            }
            else
            {
                _players[i] = new Player(Color.red, PlayerColors[i], true);
                
                var controller = Instantiate(AiPrefab, StartPositions[i].position, Quaternion.identity);    
                controller.Initialize(_players[i]);
            }
            
        }

        for (int i = 0; i < _players.Length; i++)
        {
            var player = _players[i];
            var start = StartPositions[i].position;
            for (int j = 0; j < StartUnitCount; j++)
            {
                var newUnit = Instantiate(StartUnit, start - 5 * Vector3.forward, Quaternion.identity);
                newUnit.Initialize(player);                
            }

            var newBuilding = Instantiate(StartBuilding, start, StartBuilding.transform.rotation);
            newBuilding.Initialize(player);
            newBuilding.GetComponentInChildren<MeshRenderer>().material = player.EntityMaterial;
        }
    }
    
    
}
