using Assets.Main;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Ghost
{
    public class Ghost : Enemy
    {
    
        [HideInInspector]
        public Animator Anim;
        [HideInInspector]
        public bool FacingLeft = false;
        private GhostStateMachine _stateMachine;
        [HideInInspector]
        public bool Disappear = false;

        [HideInInspector] public bool Floating = true;


        public float DetectionDistance;
        public float MaximumSpeed;
        public float DisappearTime;
        public float DisappearDistance;
        public float FloatingTime;
        public float RevealDistance;

        void Start()
        {
            FacingLeft = false;
            Anim = GetComponent<Animator>();
            _stateMachine = new GhostStateMachine(this);
        }

        public override void FixedUpdate()
        {
            _stateMachine.UpdateState();
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


        public void Flip()
        {
            FacingLeft = !FacingLeft;
            Vector3 theScale = Anim.transform.localScale;
            theScale.x *= -1;
            Anim.transform.localScale = theScale;
        }


        public Ghost()
        {
            // All of the Properties where given via Unity Editor.
        }
    }
}