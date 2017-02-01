using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Models.PickUps
{
    public class PickUpGeneral : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        private string _currentTag;
        public float Amount;
        public float DropChance;

        void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _currentTag = this.gameObject.tag;
        }

        void FixedUpdate()
        {
            _rb2d.AddForce(transform.up * -2);
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                var currentGameObject = coll.gameObject.GetComponent<SimpleScore>();
                var currentHealthObject = coll.gameObject.GetComponent<SimpleHealth>();
                var currentUpDownObject = coll.gameObject.GetComponent<SimpleUpDown>();
                if (_currentTag.Contains("Money"))
                {
                    if (currentGameObject.GainMoney((int)Amount))
                        Destroy(this.gameObject);
                }
                if (_currentTag.Contains("UpDown"))
                {
                    if (currentUpDownObject.AddUpDown((int)Amount))
                        Destroy(this.gameObject);
                }
                if (_currentTag.Contains("HeartPickUp"))
                {
                    if (currentHealthObject.GainHealth(Amount))
                        Destroy(this.gameObject);
                }
                if (_currentTag.Contains("Key"))
                {
                    if (currentGameObject.AddKey((int)Amount))
                        Destroy(this.gameObject);
                }

                Destroy(this.gameObject);

            }
        }
    }
}