using System.Linq;
using Assets.Main;
using Assets.Scripts.Models.Items;
using UnityEngine;

namespace Assets.Scripts.Models.ItemRoom
{
    public class ItemRoomController : MonoBehaviour
    {
        [HideInInspector]
        public bool ItemExists;

        [HideInInspector]
        public bool EnteredBefore;

        [HideInInspector]
        public GameObject Item;

        private GameObject _currentItem;
        private GameObject _itemPedestal;
        private const float OffSetValue = 0.36f;

        void Start()
        {
            _currentItem = Resources.FindObjectsOfTypeAll<GameObject>().ToList().Find(op => op.tag == "Item");
            if (_currentItem)
            {
                _itemPedestal = GameObject.FindGameObjectWithTag("ItemPedestal");
                _currentItem.SetActive(true);
                Vector3 offSet = new Vector3(OffSetValue, OffSetValue);
                offSet.Scale(_itemPedestal.transform.up);
                _currentItem.transform.position = _itemPedestal.transform.position + offSet;
                _currentItem.transform.rotation = _itemPedestal.transform.rotation;
                //Debug.Log("Item Found");
            }

        }

        public ItemRoomController()
        {
            ItemExists = true;
            EnteredBefore = false;
        }
    }
}