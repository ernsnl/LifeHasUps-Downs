using System.Collections.Generic;
using Assets.Scripts.Models.Items;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class SimpleItem : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> Items;

        public List<GameObject> GetItems()
        {
            return Items;
        }

        public void Start()
        {
            Items = new List<GameObject>();
        }

        public void AddItem(GameObject item)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var itemInfo = item.GetComponent<Item>();
            var playerFire = player.GetComponent<SimpleFire>();
            var playerSpeed = player.GetComponent<SimpleMovement>();
            var playerHealth = player.GetComponent<SimpleHealth>();
            Items.Add(item);
            if (itemInfo.AttackModifier != 0f)
            {
                playerFire.DamageChange(itemInfo.AttackModifier);
            }
            if (itemInfo.HpModifier > 0)
            {
                playerHealth.GainContainer((int)itemInfo.HpModifier, true);
            }
            else if (itemInfo.HpModifier < 0)
            {
                playerHealth.LoseContainer((int)itemInfo.HpModifier * -1, transform);
            }
            if (itemInfo.SpeedModifier != 0f)
            {
                playerSpeed.ChangeSpeed(itemInfo.SpeedModifier);
            }
            if (itemInfo.BulletSpeedModifier != 0)
            {
                playerFire.SpeedChange(itemInfo.BulletSpeedModifier);
            }
        }
    }
}