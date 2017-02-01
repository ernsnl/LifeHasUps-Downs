using Assets.Scripts.Character;
using Assets.Scripts.Misc.TextSlide;
using UnityEngine;

namespace Assets.Scripts.Models.Items
{
    public class Item : MonoBehaviour, IItem
    {
        [HideInInspector] public bool Spawned;

        public float SpawnChance;
        public float HpModifier;
        public float AttackModifier;
        public float SpeedModifier;
        public float BulletSpeedModifier;
        private bool _availableForKeyPress;

        [HideInInspector]
        public FloatingText CurrentGameObject;
        [HideInInspector]
        public string EnterString;

        public float OffSetAmount = 0.35f;

        private void Awake()
        {
            _availableForKeyPress = false;
            FloatingTextController.Initialize();

            // TO DO: Get Key Combination
            EnterString = "Press E to Take";           
            CurrentGameObject = FloatingTextController.CreateFloatingText(EnterString, transform);
            CurrentGameObject.SetOpacity(0);
            _availableForKeyPress = false;
        }


        private void FixedUpdate()
        {
            if(CurrentGameObject == null)
            {
                CurrentGameObject = FloatingTextController.CreateFloatingText(EnterString, transform);
                CurrentGameObject.SetOpacity(0);
            }

            var shiftVector = new Vector3(OffSetAmount, OffSetAmount);
            shiftVector.Scale(transform.up);
            Vector2 screenPosition = UnityEngine.Camera.main.WorldToScreenPoint(transform.position + shiftVector);
            CurrentGameObject.transform.position = screenPosition;
            CurrentGameObject.transform.rotation = transform.rotation;

            if (_availableForKeyPress && Input.GetButtonDown("Selective Button"))
            {
                CurrentGameObject.SetOpacity(0);
                Destroy(this.gameObject);
                var player = GameObject.FindGameObjectWithTag("Player");
                var playerItem = player.GetComponent<SimpleItem>();
                playerItem.AddItem(this.gameObject);
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