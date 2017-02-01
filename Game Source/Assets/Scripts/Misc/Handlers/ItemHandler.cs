using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Assets.Main;
using Assets.Scripts.Misc.CustomFunctions;
using Assets.Scripts.Models.Items;
using UnityEngine;
using UnityEngine.Analytics;

namespace Assets.Scripts.Misc.Handlers
{
    public class ItemHandler
    {
        private List<GameObject> _items;

        public List<GameObject> Items
        {
            get
            {
                _items = GetItems();
                return _items;
            }
        }

        private List<GameObject> GetItems()
        {
            var obj = (Resources.LoadAll<GameObject>("Prefabs/Items")).ToList();
            return obj;
        }

        public GameObject SpawnItem()
        {
            if (Items.Count == 0)
                return null;
            while (true)
            {
                this.Items.Shuffle();
                var currentItem = Items.FirstOrDefault();
                var itemInfo = currentItem.GetComponent<Item>();
                var getRandom = GameHandler.Game.Random.Range(0, 100);
                if (itemInfo.SpawnChance > getRandom)
                {
                    return currentItem;
                }
            }
            return null;
        }
    }
}