using Assets.Main;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.ClownBox
{
    public class ClownBox : Enemy
    {
        private Rigidbody2D _rb2D;
        private ClownBoxStateMachine _stateMachine;
        public float DetectionDistance;
        [HideInInspector]
        public bool _facingLeft;

        [HideInInspector] public Animator Anim;

        void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _facingLeft = true;
            _stateMachine = new ClownBoxStateMachine(this);
            Anim = GetComponent<Animator>();
        }

        public void Flip()
        {
            _facingLeft = !_facingLeft;
            Vector3 theScale = Anim.transform.localScale;
            theScale.x *= -1;
            Anim.transform.localScale = theScale;
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
        }

        public override void FixedUpdate()
        {
            //Contstant Gravity to Object. It has to be applied no matter what
            _rb2D.AddForce(transform.up * -1 * Physics2D.gravity.magnitude);
            _stateMachine.UpdateState();
        }
    }
}