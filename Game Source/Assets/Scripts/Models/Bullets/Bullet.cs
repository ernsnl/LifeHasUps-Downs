using System.ComponentModel;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.Bullets
{
    public class Bullet : MonoBehaviour
    {

        public float AppliedForce;
        public float MaxSpeed;
        public float MaximumAliveTime;

        private bool facingLeft;
        private Rigidbody2D _rb2D;
        private float _aliveTime;
        private Transform ExitLocation;
        public void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            facingLeft = true;
            _aliveTime = 0f;
            ExitLocation = GameObject.FindGameObjectWithTag("Player").transform.FindChild("FirePointer");
            _rb2D.gravityScale = 0f;
            transform.parent = null;
        }

        public void FixedUpdate()
        {
            ExitLocation = GameObject.FindGameObjectWithTag("Player").transform.FindChild("FirePointer");
            _aliveTime += Time.deltaTime;
            if (_aliveTime < MaximumAliveTime)
            {

              
                Vector3 objectPos = transform.InverseTransformPoint(ExitLocation.position);
                if (objectPos.x <= 0 && facingLeft)
                    Flip();
                else if (objectPos.x > 0 && facingLeft)
                    Flip();

                _rb2D.velocity = transform.right*MaxSpeed;
                //if(_rb2D.velocity.magnitude < MaxSpeed)
                //    _rb2D.AddForce(transform.right * AppliedForce);

                var ray = Physics2D.Raycast(transform.position, transform.right, 1f);
                //Debug.DrawRay(transform.position, transform.right, Color.blue, 1.5f);


            }
            else
            {
                _aliveTime = 0f;
                Destroy(this.gameObject);
            }
        }

        public void Flip()
        {
            facingLeft = !facingLeft;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }


        public void OnCollisionEnter2D(Collision2D coll)
        {
            if ("Player" != coll.gameObject.tag)
            {
                if (coll.gameObject.tag == "Enemy")
                {
                    var getPlayer = GameObject.FindGameObjectWithTag("Player");
                    var scoreUpdate = getPlayer.GetComponent<SimpleScore>();
                    scoreUpdate.IncreaseScore(GlobalConst.EnemyHitScore);
                }
                Destroy(this.gameObject);

            }
        }

    }
}