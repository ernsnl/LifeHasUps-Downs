using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Main;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.CustomFunctions;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Misc.Handlers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Enemies;
using Assets.Scripts.Models.ExitRoom;
using Assets.Scripts.Models.ItemRoom;
using Assets.Scripts.Models.Obstacles;
using Assets.Scripts.Models.PickUps;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class Spawner : MonoBehaviour
    {

        public void LoadLayout(List<GameObject> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {

                var currentNodeObject = tiles[i];
                Instantiate(currentNodeObject, currentNodeObject.transform.position, currentNodeObject.transform.rotation);
            }
        }

        public void CreateLevel(int minimumLevelSize, bool spawnWorld = true)
        {

            if (spawnWorld)
            {
                //Place Tiles
                var currentWorld = GameHandler.Game.World.BuildWorld(minimumLevelSize, PlayerPrefs.GetString("seedNumber"));

                for (int i = 0; i < currentWorld.Count; i++)
                {
                    // Tile Spawn
                    if (currentWorld.GraphNodes[i] != null)
                    {
                        var currentNodeObject = currentWorld.GraphNodes[i].NodeObject;

                        Vector3 pos = new Vector3(currentNodeObject.PositionX * 8,
                            currentNodeObject.PositionY * 8, 0);
                        Instantiate(currentNodeObject.Tile, pos, currentNodeObject.Tile.transform.rotation);

                    }
                }
            }
        }

        public void SpawnHero(Transform parent, GameObject selectedHero)
        {
            Instantiate(selectedHero, parent.transform.position, parent.transform.rotation);
        }

        public void SpawnExitRoom(Transform parent)
        {
            var generationPoint = parent.GetComponent<GeneratorParent>();
            var currentItemRoom = Resources.Load<GameObject>("Prefabs/Misc/Exit_Door");

            if (generationPoint.placeExit)
            {
                var selectSpawnPoint = GameHandler.Game.Random.Range(0, generationPoint.PlacementList.Count - 1);
                var selectedPoint = parent.transform.GetChild(generationPoint.PlacementList[selectSpawnPoint]);
                var selectedItemInfo = currentItemRoom.GetComponent<ExitRoom>();

                Vector3 pos = new Vector3(selectedPoint.position.x, selectedPoint.position.y, 1);
                Vector3 offSet = new Vector3(selectedItemInfo.OffSet, selectedItemInfo.OffSet, 0);
                offSet.Scale(selectedPoint.up);
                pos += offSet;
                //Adjust Their Rotation
                var copy = (GameObject)Instantiate(currentItemRoom, pos, selectedPoint.rotation);
                copy.transform.SetParent(selectedPoint);
                generationPoint.PlacementList.RemoveAt(selectSpawnPoint);
            }
        }

        public void SpawnItemRoom(Transform Parent)
        {
            var generationPoint = Parent.GetComponent<GeneratorParent>();
            var currentItemRooms = GameHandler.Game.Generation.ItemRooms;

            if (generationPoint.placeItemRoom)
            {
                var selectSpawnPoint = GameHandler.Game.Random.Range(0, generationPoint.PlacementList.Count - 1);
                var selectedPoint = Parent.transform.GetChild(generationPoint.PlacementList[selectSpawnPoint]);
                var selectTypeSpawn = GameHandler.Game.Random.Range(0, currentItemRooms.Count - 1);
                var selectedItemRoom = currentItemRooms[selectTypeSpawn];
                var selectedItemInfo = selectedItemRoom.GetComponent<ItemRoom>();

                Vector3 pos = new Vector3(selectedPoint.position.x, selectedPoint.position.y, 1);
                Vector3 offSet = new Vector3(selectedItemInfo.OffSet, selectedItemInfo.OffSet, 0);
                offSet.Scale(selectedPoint.up);
                pos += offSet;
                //Adjust Their Rotation
                var copy = (GameObject)Instantiate(selectedItemRoom, pos, selectedPoint.rotation);
                var itemDoorInfo = copy.GetComponent<ItemRoom>();
                if (GameHandler.Game.CurrentLevel > 1)
                {
                    itemDoorInfo.KeyAmount = GameHandler.Game.Random.Range(0, GameHandler.Game.CurrentLevel);
                    itemDoorInfo.RequireKey = itemDoorInfo.KeyAmount > 0;
                }
                copy.transform.SetParent(selectedPoint);
                generationPoint.PlacementList.RemoveAt(selectSpawnPoint);
            }
        }

        public void SpawnObstacle(Transform Parent)
        {
            var generationPoint = Parent.GetComponent<GeneratorParent>();
            var currentObstacles = GameHandler.Game.Generation.Obstacles;
            var iter = generationPoint.MaximumObstacleAmount;
            while (iter != 0)
            {
                var spawned = GameHandler.Game.Random.Range(0, 1);
                if (spawned > 0)
                {
                    var selectSpawnPoint = GameHandler.Game.Random.Range(0, generationPoint.PlacementList.Count - 1);
                    var selectedPoint = Parent.transform.GetChild(generationPoint.PlacementList[selectSpawnPoint]);
                    var selectTypeSpawn = GameHandler.Game.Random.Range(0, currentObstacles.Count - 1);
                    var selectedObstacle = currentObstacles[selectTypeSpawn];
                    var selectedObstacleInfo = selectedObstacle.GetComponent<Obstacle>();

                    Vector3 pos = new Vector3(selectedPoint.position.x, selectedPoint.position.y, 0);
                    Vector3 offSet = new Vector3(selectedObstacleInfo.OffSet, selectedObstacleInfo.OffSet, 0);
                    offSet.Scale(selectedPoint.up);
                    pos += offSet;
                    //Adjust Their Rotation
                    var copy = (GameObject)Instantiate(selectedObstacle, pos, selectedPoint.rotation);
                    if (selectedObstacle.name.Contains("Stone"))
                        generationPoint.FlowerPlaceList.Add(generationPoint.PlacementList[selectSpawnPoint]);
                    copy.transform.SetParent(selectedPoint);
                    copy.transform.position = pos;
                    generationPoint.PlacementList.RemoveAt(selectSpawnPoint);
                }
                iter--;
            }

        }

        public void SpawnObstacle(TileHandler Tile)
        {
            if (Tile.Tile.transform.FindChild("ObstacleGeneration") != null)
                SpawnObstacle(Tile.Tile.transform.FindChild("ObstacleGeneration"));
        }

        public void SpawnEnemy(Transform Parent)
        {
            var generationPoint = Parent.GetComponent<GeneratorParent>();
            var enemies = GameHandler.Game.Generation.Enemies;
            var iter = generationPoint.MaximumEnemyAmount;
            while (iter != 0)
            {
                var spawned = GameHandler.Game.Random.Range(0, 1);
                if (spawned > 0 && generationPoint.PlacementList.Count > 0)
                {
                    var selectSpawnPoint = GameHandler.Game.Random.Range(0, generationPoint.PlacementList.Count - 1);
                    var selectedPoint = Parent.transform.GetChild(generationPoint.PlacementList[selectSpawnPoint]);
                    var selectTypeSpawn = GameHandler.Game.Random.Range(0, enemies.Count - 1);
                    var selectedEnemy = enemies[selectTypeSpawn];
                    var selectedEnemyInfo = selectedEnemy.GetComponent<Enemy>();

                    Vector3 pos = new Vector3(selectedPoint.position.x, selectedPoint.position.y, 0);
                    Vector3 offSet = new Vector3(selectedEnemyInfo.OffSet, selectedEnemyInfo.OffSet, 0);
                    offSet.Scale(selectedPoint.up);
                    pos += offSet;
                    //Adjust Their Rotation
                    var copy = (GameObject)Instantiate(selectedEnemy, pos, selectedPoint.rotation);
                    copy.transform.SetParent(selectedPoint, true);
                    generationPoint.PlacementList.RemoveAt(selectSpawnPoint);


                }
                iter--;
            }
        }

        public void SpawnFlowers(Transform Parent)
        {
            var generationPoint = Parent.GetComponent<GeneratorParent>();
            var flowerSpriteList = GameHandler.Game.Generation.Flowers;
            while (generationPoint.FlowerPlaceList.Count != 0)
            {
                var selectedPoint = Parent.transform.GetChild(generationPoint.FlowerPlaceList[0]);
                for (int i = 0; i < GlobalConst.FlowerSpawnAmount; i++)
                {
                    var visualPresentation = GameHandler.Game.Random.Range(0, 1);
                    var visualDirection = GameHandler.Game.Random.Range(0, 1);
                    var selectTypeSpawn = GameHandler.Game.Random.Range(0, flowerSpriteList.Length - 1);
                    Vector3 pos = new Vector3(selectedPoint.position.x, selectedPoint.position.y, -visualPresentation);
                    var selectedFlower = flowerSpriteList[selectTypeSpawn];
                    GameObject flower = new GameObject("Flower");
                    flower.AddComponent<SpriteRenderer>().sprite = (Sprite)selectedFlower;
                    Vector3 offSet = new Vector3(GlobalConst.FlowerOffSet, GlobalConst.FlowerOffSet, 0);
                    offSet.Scale(selectedPoint.up);
                    Vector3 offSetFlowerField = new Vector3(GlobalConst.FlowerStoneOffset + GlobalConst.FlowerFieldOffSet * i,
                        GlobalConst.FlowerStoneOffset + GlobalConst.FlowerFieldOffSet * i, 0);
                    offSetFlowerField.Scale(visualDirection == 0 ? selectedPoint.right : -selectedPoint.right);
                    pos += offSetFlowerField;
                    pos += offSet;
                    flower.transform.position = pos;
                    flower.transform.rotation = selectedPoint.rotation;
                    flower.transform.SetParent(selectedPoint);
                }
                generationPoint.FlowerPlaceList.RemoveAt(0);
            }
        }
        public bool CreatePickUp(Transform parent)
        {
            var direction = GameHandler.Game.Random.Range(0, 1);
            var pickUps = GameHandler.Game.Generation.PickUps;
            pickUps.Shuffle();
            var dropChance = GameHandler.Game.Random.Range(0, 100);
            var firstPickUp = pickUps.FirstOrDefault();
            var getDropChance = firstPickUp.GetComponent<PickUpGeneral>();
            if (getDropChance.DropChance * 100 >= dropChance)
            {
                var copyPickUp = (GameObject)Instantiate(firstPickUp, parent.position, parent.rotation);

                copyPickUp.GetComponent<Rigidbody2D>().AddForce(parent.transform.up * 120);
                copyPickUp.GetComponent<Rigidbody2D>().AddForce(parent.transform.right * 120 * (direction > 0 ? 1 : -1));

                return true;
            }
            else
            {
                dropChance = GameHandler.Game.Random.Range(0, 75);
                var oneMoney = pickUps.Find(op => op.name.Contains("Money_I"));
                getDropChance = oneMoney.GetComponent<PickUpGeneral>();
                if (getDropChance.DropChance * 100 > dropChance)
                {
                    var copyPickUp = (GameObject)Instantiate(firstPickUp, parent.position, parent.rotation);

                    copyPickUp.GetComponent<Rigidbody2D>().AddForce(parent.transform.up * 120);
                    copyPickUp.GetComponent<Rigidbody2D>().AddForce(parent.transform.right * 120 * (direction > 0 ? 1 : -1));

                    return true;
                }
            }
            return false;

        }
    }
}