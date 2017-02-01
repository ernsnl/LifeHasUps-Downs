using System;
using Assets.Main;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.Bullets;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Tank
{
    public class Tank : Enemy
    {
        // For Physics 
        private Rigidbody2D _rb2D;
        // For Animation 
        [HideInInspector]
        public Animator Anim;
        [HideInInspector]
        public bool FacingLeft = true;
        // For State Machine
        private TankStateMachine _stateMachine;
        [HideInInspector]
        public Vector2 LastKnownCollision;

        public float RayDistance;
        public float ChaseBufferDistance;
        public float AlertStateTimer;



        void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            _stateMachine = new TankStateMachine(this);
        }

        public override void ThrowTrigger(GlobalEnums trigg, bool enterExit)
        {
            _stateMachine.ThrowTrigger(trigg, enterExit);
        }


        #region Trigger

        public override void OnTriggerStay2D(Collider2D coll)
        {

        }

        public override void OnTriggerEnter2D(Collider2D coll)
        {

        }

        public override void OnTriggerExit2D(Collider2D coll)
        {

        }

        #endregion

        public void Fire()
        {
            FirePointer.rotation = transform.rotation;
            var bullet = (GameObject)Instantiate(Bullet, FirePointer.position, this.transform.rotation, this.transform);
        }

        #region Collision 

        public override void OnCollisionStay2D(Collision2D coll)
        {

        }

        public override void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player_Bullet")
            {
                BaseHealth -= SimpleFire.GetFireDamage();
                if (BaseHealth < 0f)
                {
                    GameHandler.Game.Spawn.CreatePickUp(this.transform);
                    Destroy(this.gameObject);
                }
            }
            _stateMachine.OnCollisionEnter2D(coll);
        }

        public override void OnCollisionExit2D(Collision2D coll)
        {

        }
        #endregion

        public override void FixedUpdate()
        {
            //Contstant Gravity to Object. It has to be applied no matter what
            _rb2D.AddForce(transform.up * -1 * Physics2D.gravity.magnitude);
            Anim.SetBool("Movement", true);
            _stateMachine.UpdateState();

        }

        public Rigidbody2D GetRb2D()
        {
            return _rb2D;
        }

        public Animator GetAnimator()
        {
            return Anim;
        }

        public void Flip()
        {
            FacingLeft = !FacingLeft;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            //Vector3 theScale = transform.localScale;
            //theScale.x *= -1;
            //transform.localScale = theScale;

            //float rotation = transform.FindChild("FirePointer").rotation.y;
            //rotation += 180;
            //transform.FindChild("FirePointer").rotation = Quaternion.Euler(0, rotation, 0);

        }


        public Tank()
        {
            // All of the Properties where given via Unity Editor.
        }

    }
}