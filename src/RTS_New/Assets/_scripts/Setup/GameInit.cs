﻿using UnityEngine;

public class GameInit : MonoBehaviour
{
    public int StartUnitCount = 1;
    public int PLayerCount = 1;
    public MenuDataUnit StartUnit;
    public Building StartBuilding;
    public PlayerController PlayerPrefab;
    public PlayerController AiPrefab;
    
    public Transform[] StartPositions;
    
    private Player[] _players;

    //public Dictionary<Color, Texture> entityTextures;
    public Material[] PlayerColors;

    public Material[] PlayerUnitColors;
    
    private void Awake()
    {
        _players = new Player[PLayerCount];
        var realPlayerSet = false;
        for (int i = 0; i < PLayerCount; i++)
        {
            if (!realPlayerSet)
            {
                _players[i] = new Player(Color.blue, PlayerColors[i], PlayerUnitColors[i], false);
                var controller = Instantiate(PlayerPrefab, StartPositions[i].position, Quaternion.identity);
                controller.Initialize(_players[i]);
                realPlayerSet = true;
            }
            else
            {
                _players[i] = new Player(Color.red, PlayerColors[i], PlayerUnitColors[i], true);
                
                var controller = Instantiate(AiPrefab, StartPositions[i].position, Quaternion.identity);    
                controller.Initialize(_players[i]);
            }
            
        }

        var unitFoodUse = StartUnit.Data.Food;
        
        for (int i = 0; i < _players.Length; i++)
        {
            
            var player = _players[i];
            var start = StartPositions[i].position;
            
            var newBuilding = Instantiate(StartBuilding, start, StartBuilding.transform.rotation);
            newBuilding.Initialize(player);


            for (int j = 0; j < StartUnitCount; j++)
            {

                player.ChangeFoodUsage(unitFoodUse);
                
                StartUnit.Complete(newBuilding);                
            }

        }
    }
    
    
}
