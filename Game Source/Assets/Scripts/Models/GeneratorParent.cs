using System.Collections.Generic;
using System.Linq;
using Assets.Main;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class GeneratorParent : MonoBehaviour
    {
        public GameObject ParentGameObject;
        public int MaximumObstacleAmount = 4;
        public int MaximumEnemyAmount = 3;
        public List<int> PlacementList = new List<int>();
        public List<int> FlowerPlaceList = new List<int>();

        [HideInInspector]
        public bool placeItemRoom = false;
        [HideInInspector]
        public bool placeHero = false;

        [HideInInspector] public bool placeExit = false;

        public void InitPreGeneration()
        {
            PlacementList = Enumerable.Range(0, ParentGameObject.transform.childCount).ToList();
            FlowerPlaceList = new List<int>();
            MaximumEnemyAmount = 3;
        }

        public void Start()
        {
            if (!placeHero)
            {
                InitPreGeneration();
                GameHandler.Game.Spawn.SpawnItemRoom(ParentGameObject.transform);
                GameHandler.Game.Spawn.SpawnExitRoom(ParentGameObject.transform);
                GameHandler.Game.Spawn.SpawnObstacle(ParentGameObject.transform);
                GameHandler.Game.Spawn.SpawnEnemy(ParentGameObject.transform);
                GameHandler.Game.Spawn.SpawnFlowers(ParentGameObject.transform);
            }
        }

    }
}