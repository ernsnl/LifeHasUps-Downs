using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Main;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.Handlers;
using Assets.Scripts.Models;
using Assets.Scripts.Spawner;


public class PrefabSpawn : MonoBehaviour
{
    public GameObject gameHandler;
    public int MinimumLevelSize = 4;
    // Use this for initialization


    private void Awake()
    {
        
        if (GameHandler.Game == null)
        {
            Instantiate(gameHandler);
            GameHandler.Game.Begin = true;
        }

        GameHandler.Game.MinimumLevelSize = MinimumLevelSize;
        if (GameHandler.Game.ActivatePrevWorld)
        {
            GameHandler.Game.Begin = false;
            GameHandler.Game.NewLevel = false;
            GameHandler.Game.ActivatePrevWorld = false;
            GameHandler.Game.ActiviatePrevWorld();
        }
        if (GameHandler.Game.NewLevel)
        {
            GameHandler.Game.Begin = false;
            GameHandler.Game.NewLevel = false;
            GameHandler.Game.ActivatePrevWorld = false;
            GameHandler.Game.IncreaseLevel();
            GameHandler.Game.GotoNextLevel();
        }

        if (GameHandler.Game.Begin)
        {
            GameHandler.Game.Begin = false;
            GameHandler.Game.NewLevel = false;
            GameHandler.Game.ActivatePrevWorld = false;
            GameHandler.Game.SpawnNewWorld();
        }

        
    }
}
