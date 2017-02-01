using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Misc.Handlers
{
    public class GenerationHandler : Component
    {

        private List<GameObject> _obstacles;
        private List<GameObject> _enemies;
        private Sprite[] _flowerSprites;
        private List<GameObject> _itemRooms;
        private List<GameObject> _pickUps;

        public Sprite[] Flowers
        {
            get
            {
                if (_flowerSprites == null)
                    _flowerSprites = GetFlowers();
                return _flowerSprites;
            }
        }

        public List<GameObject> PickUps
        {
            get
            {
                if (_pickUps == null)
                    _pickUps = GetPickUps();
                return _pickUps;
            }
        }

        public List<GameObject> ItemRooms
        {
            get
            {
                if (_itemRooms == null)
                    _itemRooms = GetItemRooms();
                return _itemRooms;
            }
        }

        public List<GameObject> Obstacles
        {
            get
            {
                if (_obstacles == null)
                    _obstacles = GetObstacles();
                return _obstacles;
            }
        }

        public List<GameObject> Enemies
        {
            get
            {
                if (_enemies == null)
                    _enemies = GetEnemies();
                return _enemies;
            }
        }

        private List<GameObject> GetPickUps()
        {
            List<GameObject> pickUp = new List<GameObject>();
            string[] listofPickUps = { "Key", "Money_I", "Money_V", "Money_X", "UpDown" };
            pickUp = Resources.LoadAll<GameObject>("Prefabs/Misc/PickUps").ToList();
            return pickUp;
        }


        private List<GameObject> GetEnemies()
        {
            List<GameObject> enm = new List<GameObject>();
            string[] listOfEnemies = { "BeeHive", "ClownBox", "Ghost", "Spider", "Tank" };

            foreach (var enemy in listOfEnemies)
            {
                var _enm = (GameObject)(Resources.Load("Prefabs/Enemy/" + enemy));
                enm.Add(_enm);
            }
            return enm;
        }

        private List<GameObject> GetObstacles()
        {
            List<GameObject> obs = new List<GameObject>();

            string[] listOfObstacles =
                {
                    "Fire Hydrant",
                    "Stone",
                    "QuestionBox",
                    "Furnace",
                    "Football"
                };

            foreach (var obstacle in listOfObstacles)
            {
                var _obs = (GameObject)(Resources.Load("Prefabs/Obstacles/" + obstacle));
                obs.Add(_obs);
            }

            return obs;
        }

        private List<GameObject> GetItemRooms()
        {
            List<GameObject> itemRooms = new List<GameObject>();

            string[] listOfItemRooms =
                {
                    "ItemRoom"
                };

            foreach (var itemRoom in listOfItemRooms)
            {
                var r = (GameObject)(Resources.Load("Prefabs/Misc/" + itemRoom));
                itemRooms.Add(r);
            }

            return itemRooms;
        }

        private Sprite[] GetFlowers()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(@"Sprites/Flowers");
            return sprites;
        }

    }
}