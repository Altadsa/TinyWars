using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiPlayerResources : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;
    
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _lumberText;
    [SerializeField] private TMP_Text _ironText;
    [SerializeField] private TMP_Text _foodText;

    //TODO Fix
    private void Start()
    {
        var player = _controller.Player;
        UpdateResources(player.PlayerResources, player.MaxFood);
        player.ResourcesUpdated += UpdateResources;
    }

    private void UpdateResources(ResourceData data, int maxFood)
    {
        _goldText.text = data.Gold.ToString();
        _lumberText.text = data.Lumber.ToString();
        _ironText.text = data.Iron.ToString();
        _foodText.text = $"{data.Food.ToString()}/{maxFood}";
    }
}
