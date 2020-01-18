using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class TestModifiers : MonoBehaviour
{
    private Player Player;
    
    private void Start()
    {
        Player = FindObjectOfType<PlayerController>().Player;
    }

    public void ChangeModifier(float value)
    {
        Debug.Log("Updating Modifier");
        Player.ChangeModifier(UnitType.ARCHER, Modifier.Armour, value);
    }
}
