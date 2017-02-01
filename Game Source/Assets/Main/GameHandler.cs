using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.Handlers;
using Assets.Scripts.Models;
using Assets.Scripts.Spawner;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Main
{
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler Game = null;
        public int CurrentLevel = 1;
        public int MinimumLevelSize = 4;

        private bool _generateItemRoom;
        private bool _spawnHero;
        private bool _spawnExit;

        private WorldHandler _world;
        private TileHandler _tile;
        private RandomHandler _random;
        private HeroHandler _hero;
        private GenerationHandler _generation;
        private ItemHandler _item;
        private Spawner _spawn;

        [HideInInspector]
        public bool ActivatePrevWorld;
        [HideInInspector]
        public bool NewLevel;
        [HideInInspector]
        public bool Begin;
        [HideInInspector]
        public GameObject Player
        {
            get { return GameObject.FindGameObjectWithTag("Player"); }
        }

        public WorldHandler World
        {
            get { return _world; }
        }

        public TileHandler Tile
        {
            get { return _tile; }
        }

        public RandomHandler Random
        {
            get { return _random; }
        }

        public HeroHandler Hero
        {
            get { return _hero; }
        }

        public GenerationHandler Generation
        {
            get { return _generation; }
        }

        public ItemHandler Item
        {
            get { return _item; }
        }

        public Spawner Spawn
        {
            get
            {
                return _spawn;
            }
        }

        public void InitGame()
        {
            _world = new WorldHandler();
            _tile = new TileHandler();
            _random = new RandomHandler();
            _hero = new HeroHandler();
            _generation = new GenerationHandler();
            _spawn = new Spawner();
            _item = new ItemHandler();

        }

        public void SpawnNewWorld()
        {
            _generateItemRoom = false;
            _spawnHero = false;
            _spawnExit = false;

            if (String.IsNullOrEmpty(PlayerPrefs.GetString("seedNumber")))
            {
                PlayerPrefs.SetString("seedNumber", GameHandler.Game.Random.RandomString(12));
            }

            Debug.Log(PlayerPrefs.GetString("seedNumber"));
            DestroyUnnecessary();

            GameHandler.Game.Spawn.CreateLevel(MinimumLevelSize);

            var generatedTiles = GameObject.FindGameObjectsWithTag("GeneratedTile");
            foreach (var tile in generatedTiles)
            {
                var obstacleGeneration = tile.transform.FindChild("ObstacleGeneration").GetComponent<GeneratorParent>();
                obstacleGeneration.placeExit = false;
                obstacleGeneration.placeHero = false;
                obstacleGeneration.placeItemRoom = false;
            }

            var itemLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
            var heroLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
            var exitLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);

            if (!_spawnHero)
            {
                var gateInfo = generatedTiles[heroLocation].GetComponent<TileGateModel>();
                while (gateInfo.southGate != 0)
                {
                    heroLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
                    gateInfo = generatedTiles[heroLocation].GetComponent<TileGateModel>();
                }
                var currentHeros = GameHandler.Game.Hero.Heros;
                var copy = Instantiate(currentHeros[0]);
                copy.transform.position = generatedTiles[heroLocation].transform.position;
                copy.transform.rotation = generatedTiles[heroLocation].transform.rotation;
                copy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                var selectedTile = generatedTiles[heroLocation];
                var generationPoints = selectedTile.transform.FindChild("ObstacleGeneration");
                generationPoints.GetComponent<GeneratorParent>().placeHero = true;
            }

            if (!_generateItemRoom)
            {
                while (heroLocation == itemLocation)
                {
                    itemLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
                }
                var selectedTile = generatedTiles[itemLocation];
                var generationPoints = selectedTile.transform.FindChild("ObstacleGeneration");
                generationPoints.GetComponent<GeneratorParent>().placeItemRoom = true;
            }

            if (!_spawnExit)
            {
                while (heroLocation == exitLocation)
                {
                    exitLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
                }
                var selectedTile = generatedTiles[exitLocation];
                var generationPoints = selectedTile.transform.FindChild("ObstacleGeneration");
                generationPoints.GetComponent<GeneratorParent>().placeExit = true;
            }
        }

        public void IncreaseLevel()
        {
            CurrentLevel += 1;
            if (CurrentLevel == 5)
            {
                Time.timeScale = 0;
                  var  currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                  var  currentEndGame = currentCanvas.transform.FindChild("EndGamePanel");
                
                if (currentEndGame)
                {
                    currentEndGame.gameObject.SetActive(true);
                    var scoreUi = currentEndGame.FindChild("Score");
                    var seedUi = currentEndGame.FindChild("Seed");
                    scoreUi.FindChild("ScoreValue").GetComponent<UnityEngine.UI.Text>().text =
                        Player.GetComponent<SimpleScore>().GetScore().ToString();
                    var seedString = GameHandler.Game.Random.GetSeedString().ToUpper();
                    for (int i = 4; i <= seedString.Length; i += 4)
                    {
                        seedString = seedString.Insert(i, "-");
                        i++;
                    }
                    seedString = seedString.Trim('-');
                    seedUi.FindChild("SeedValue").GetComponent<Text>().text = seedString;
                }
            }
            MinimumLevelSize *= CurrentLevel;
        }

        public void DestroyUnnecessary()
        {
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                DestroyImmediate(enemy);
            }

            foreach (var tile in GameObject.FindGameObjectsWithTag("GeneratedTile"))
            {
                DestroyImmediate(tile);
            }
            DestroyImmediate(GameObject.Find("ItemSpawned"));
            DestroyImmediate(GameObject.FindGameObjectWithTag("Item"));
            var unClaimedItem = Resources.FindObjectsOfTypeAll<GameObject>().ToList().Find(op => op.tag == "Item" && op.activeInHierarchy == false && op.activeSelf == false);
            if (unClaimedItem)
                DestroyImmediate(unClaimedItem);
        }

        public void GotoNextLevel()
        {
            _generateItemRoom = false;
            _spawnHero = false;
            _spawnExit = false;

            DontDestroyOnLoad(Game.Player);
            DestroyUnnecessary();

            GameHandler.Game.Spawn.CreateLevel(MinimumLevelSize);
            var generatedTiles = GameObject.FindGameObjectsWithTag("GeneratedTile");
            foreach (var tile in generatedTiles)
            {
                var obstacleGeneration = tile.transform.FindChild("ObstacleGeneration").GetComponent<GeneratorParent>();
                obstacleGeneration.placeExit = false;
                obstacleGeneration.placeHero = false;
                obstacleGeneration.placeItemRoom = false;
            }

            var itemLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
            var heroLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
            var exitLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);

            if (!_spawnHero)
            {
                var gateInfo = generatedTiles[heroLocation].GetComponent<TileGateModel>();
                while (gateInfo.southGate != 0)
                {
                    heroLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
                    gateInfo = generatedTiles[heroLocation].GetComponent<TileGateModel>();
                }
                var currentHero = Game.Player;
                currentHero.transform.position = generatedTiles[heroLocation].transform.position;
                currentHero.transform.rotation = generatedTiles[heroLocation].transform.rotation;
                currentHero.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                var selectedTile = generatedTiles[heroLocation];
                var generationPoints = selectedTile.transform.FindChild("ObstacleGeneration");

                var generationPointInfo = generationPoints.GetComponent<GeneratorParent>();
                generationPointInfo.placeHero = true;
            }

            if (!_generateItemRoom)
            {
                while (heroLocation == itemLocation)
                {
                    itemLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
                }
                var selectedTile = generatedTiles[itemLocation];
                var generationPoints = selectedTile.transform.FindChild("ObstacleGeneration");
                var generationPointInfo = generationPoints.GetComponent<GeneratorParent>();
                generationPointInfo.placeItemRoom = true;
            }

            if (!_spawnExit)
            {
                while (heroLocation == exitLocation)
                {
                    exitLocation = GameHandler.Game.Random.Range(0, generatedTiles.Length - 1);
                }
                var selectedTile = generatedTiles[exitLocation];
                var generationPoints = selectedTile.transform.FindChild("ObstacleGeneration");
                var generationPointInfo = generationPoints.GetComponent<GeneratorParent>();
                generationPointInfo.placeExit = true;
            }

        }

        public void ActiviatePrevWorld()
        {
            var generatedTile = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (var tile in generatedTile)
            {
                var tileX = (GameObject)tile;
                if (!tileX.activeInHierarchy && tileX.tag == "GeneratedTile")
                {
                    tileX.gameObject.SetActive(true);
                    var ObsPoints = tileX.transform.FindChild("ObstacleGeneration");
                    foreach (Transform child in ObsPoints)
                    {
                        foreach (Transform point in child)
                        {
                            if (point.gameObject.tag == "ItemDoor")
                            {
                                var ItemRoom = child;
                                var Offset = new Vector3(Player.GetComponent<SimpleMovement>().offSetY, Player.GetComponent<SimpleMovement>().offSetY);
                                Offset.Scale(ItemRoom.transform.up);

                                Player.transform.position = ItemRoom.transform.position + Offset;
                                Player.transform.rotation = ItemRoom.transform.rotation;
                            }
                        }
                    }
                }
            }
        }

        public void ToItemRoom()
        {
            DontDestroyOnLoad(Player);
            foreach (var tile in GameObject.FindGameObjectsWithTag("GeneratedTile"))
            {
                DontDestroyOnLoad(tile);
                tile.gameObject.SetActive(false);            
            }
        }

        void Awake()
        {
            if (Game == null)
                Game = this;

            else if (Game != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            InitGame();
        }
    }
}