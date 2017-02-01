using Assets.Scripts.Character;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.Enemies.Enemy_Obj.Tank;
using UnityEngine;

namespace Assets.Scripts.Models.Bullets
{
    public class TankBullet : MonoBehaviour
    {
        public float AppliedForce;
        public float BulletSpeed;

        private bool facingLeft;

        private Rigidbody2D _rb2D;

        public float AliveTime;
        private float _timeCounter;
        private float _direction;

        public void Awake()
        {
            var parentInfo = transform.parent.GetComponent<Tank>();
            _rb2D = GetComponent<Rigidbody2D>();
            facingLeft = true;
            _timeCounter = 0f;
            _direction = parentInfo.FacingLeft ? -1 : 1;
            transform.parent = null;
        }

        public void FixedUpdate()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter < AliveTime)
            {
                if (BulletSpeed > _rb2D.velocity.magnitude)
                    _rb2D.AddForce(transform.right * AppliedForce * _direction);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                var getPlayer = GameObject.FindGameObjectWithTag("Player");
                var healthUpdate = getPlayer.GetComponent<SimpleHealth>();
                healthUpdate.LoseHealth(0.5f, this.transform);
            }
            Destroy(this.gameObject);

        }
    }
}