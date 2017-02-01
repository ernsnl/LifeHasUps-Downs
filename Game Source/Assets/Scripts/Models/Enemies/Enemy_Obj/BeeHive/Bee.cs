using Assets.Main;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.BeeHive
{
    public class Bee : Enemy
    {
        private Animator Anim;
        public BeeType Type;
        public int Flip;
        private Rigidbody2D _rigidbody2D;

        public override void OnTriggerEnter2D(Collider2D coll)
        {
        }

        public override void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player_Bullet")
            {
                BaseHealth -= SimpleFire.GetFireDamage();
                if (BaseHealth < 0f)
                {
                    Destroy(this.gameObject);
                }
            }

            if (coll.gameObject.tag == "Obstacle")
            {
                FlipFunc();
            }
        }

        public void Start()
        {
            Anim = GetComponent<Animator>();
            Anim.SetBool("Flying", true);
            Flip = GameHandler.Game.Random.Range(0, 1) > 0 ? 1 : -1;
            if (Flip == 1 && Type == BeeType.Worker)
            {
                FlipFunc();
                Flip = 1;
            }
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdate()
        {
            if (BeeType.Worker == Type)
            {
               
                if (_rigidbody2D.velocity.magnitude < MaxSpeed)
                    _rigidbody2D.AddForce(transform.right * Flip * MovementForce);

                var rayCastDown = Physics2D.Raycast(transform.position, -1*transform.up, 2,
                    1 << LayerMask.NameToLayer("TerrainLayerMask"));

                Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, rayCastDown.normal);
                Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5);

                transform.rotation = Quaternion.Euler(0,0,finalRotation.eulerAngles.z);
                transform.position = (Vector3)rayCastDown.point + transform.up*OffSet;
            }
        }

        public void FlipFunc()
        {
            Flip *= -1;
            Vector3 scale = Anim.transform.localScale;
            scale.x *= -1;
            Anim.transform.localScale = scale;
        }
    }
}