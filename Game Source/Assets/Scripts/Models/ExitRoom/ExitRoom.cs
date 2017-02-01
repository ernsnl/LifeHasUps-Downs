using Assets.Main;
using Assets.Scripts.Misc.TextSlide;
using UnityEngine;
namespace Assets.Scripts.Models.ExitRoom
{
    public class ExitRoom : MonoBehaviour
    {
        public float OffSet;
        [HideInInspector]
        public FloatingText CurrentGameObject;
        [HideInInspector]
        public string EnterString;
        private bool _canGoThrough;
        private int _numberOfEnemies;
        private int _requiredEnemy;
        public float TextOffSetAmount;
        [HideInInspector]
        public bool AvailableForKeyPress;

        void Start()
        {
            FloatingTextController.Initialize();
            _numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            _requiredEnemy = _numberOfEnemies / 2;

            _canGoThrough = _requiredEnemy == 0;
            AvailableForKeyPress = false;

            EnterString = _requiredEnemy > 0 ? "Press E [" + _requiredEnemy + " Enemy to Progress]" : "Press E";
            CurrentGameObject = FloatingTextController.CreateFloatingText(EnterString, transform);
            CurrentGameObject.SetOpacity(0);


        }

        void FixedUpdate()
        {
            if (CurrentGameObject == null)
            {
                CurrentGameObject = FloatingTextController.CreateFloatingText(EnterString, transform);
                CurrentGameObject.SetOpacity(0);
            }

            var shiftVector = new Vector3(TextOffSetAmount, TextOffSetAmount);
            shiftVector.Scale(transform.up);
            Vector2 screenPosition = UnityEngine.Camera.main.WorldToScreenPoint(transform.position + shiftVector);
            CurrentGameObject.transform.position = screenPosition;
            CurrentGameObject.transform.rotation = transform.rotation;

            if (_canGoThrough && Input.GetButtonDown("Selective Button") && AvailableForKeyPress)
            {
                CurrentGameObject.SetOpacity(0);
                DontDestroyOnLoad(GameHandler.Game.Player);
                GameHandler.Game.NewLevel = true;
                GameHandler.Game.IncreaseLevel();
                GameHandler.Game.GotoNextLevel();
            }

        }

        public void KilledEnemy()
        {
            if (_requiredEnemy > 0)
                _requiredEnemy -= 1;
            EnterString = _requiredEnemy > 0 ? "Press E [" + _requiredEnemy + " Enemy to Progress]" : "Press E";

            if (CurrentGameObject == null)
            {
                CurrentGameObject = FloatingTextController.CreateFloatingText(EnterString, transform);
                if (CurrentGameObject)
                    CurrentGameObject.SetOpacity(0);
            }
            if (CurrentGameObject)
                CurrentGameObject.SetText(EnterString);
            _canGoThrough = _requiredEnemy == 0;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                CurrentGameObject.SetOpacity(255);
                AvailableForKeyPress = true;
            }
        }
        private void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                CurrentGameObject.SetOpacity(0);
                AvailableForKeyPress = false;
            }
        }
    }
}