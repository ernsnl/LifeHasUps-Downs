using System.Linq;
using Assets.Main;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.TextSlide;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Models.ItemRoom
{
    public class ItemRoom : MonoBehaviour
    {
        public float OffSet;
        public bool RequireKey;
        public int KeyAmount;
        public float OffsetAmount;
        public bool ReturnLevel;
        public float SceneInfo;

        [HideInInspector]
        public ItemRoomController ItemRoomController;

        [HideInInspector]
        public FloatingText CurrentGameObject;

        [HideInInspector]
        public string EnterString;

        [HideInInspector]
        private bool _availableForKeyPress;

        private bool _itemSpawned;

        void Start()
        {
            // TO DO: Initialize At Game Handler
            FloatingTextController.Initialize();

            // TO DO: Get Key Combination
            EnterString = "Press E";
            if (RequireKey && KeyAmount > 0)
                EnterString += " [Require " + (KeyAmount == 1 ? "a key" : (KeyAmount.ToString() + " keys")) + "]";

            CurrentGameObject = FloatingTextController.CreateFloatingText(EnterString, transform);
            CurrentGameObject.SetOpacity(0);

            ItemRoomController = new ItemRoomController();
            _availableForKeyPress = false;
            _itemSpawned = GameObject.Find("ItemSpawned") != null;
        }

        void FixedUpdate()
        {
            if (CurrentGameObject == null)
            {
                CurrentGameObject = FloatingTextController.CreateFloatingText(EnterString, transform);
                CurrentGameObject.SetOpacity(0);
            }

            var shiftVector = new Vector3(OffsetAmount, OffsetAmount);
            shiftVector.Scale(transform.up);
            Vector2 screenPosition = UnityEngine.Camera.main.WorldToScreenPoint(transform.position + shiftVector);
            CurrentGameObject.transform.position = screenPosition;
            CurrentGameObject.transform.rotation = transform.rotation;

            if (_availableForKeyPress && Input.GetButtonDown("Selective Button"))
            {
                var score = GameHandler.Game.Player.GetComponent<SimpleScore>();
                if (!_itemSpawned)
                {
                    var itemSpawned = new GameObject("ItemSpawned");
                    DontDestroyOnLoad(itemSpawned);
                    var spawnedItem = GameHandler.Game.Item.SpawnItem();
                    var copyItem = Instantiate(spawnedItem);
                    _itemSpawned = true;
                    copyItem.SetActive(false);
                    DontDestroyOnLoad(copyItem);
                }
                if (!ReturnLevel)
                {
                    if (!RequireKey)
                    {
                        GameHandler.Game.ToItemRoom();
                        SceneManager.LoadScene("ItemRoom", LoadSceneMode.Single);
                    }
                    else
                    {
                        if (score.GetCurrentKey() >= KeyAmount)
                        {
                            score.RemoveKey(KeyAmount);
                            RequireKey = false;
                            EnterString = "Press E";
                            CurrentGameObject.SetText(EnterString);
                            GameHandler.Game.ToItemRoom();                          
                            SceneManager.LoadScene("ItemRoom", LoadSceneMode.Single);
                        }
                    }
                }
                else
                {
                    GameHandler.Game.ActivatePrevWorld = true;
                    var item = GameObject.FindGameObjectWithTag("Item");
                    if(item)
                        item.SetActive(false);
                    SceneManager.LoadScene("Minimum Scene", LoadSceneMode.Single);    
                }
                //Debug.Log("Button pressed");
            }
        }



        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                CurrentGameObject.SetOpacity(255);
                _availableForKeyPress = true;
            }
        }
        private void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                CurrentGameObject.SetOpacity(0);
                _availableForKeyPress = false;
            }
        }
    }
}