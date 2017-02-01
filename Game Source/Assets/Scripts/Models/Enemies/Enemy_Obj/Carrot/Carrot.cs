using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Carrot
{
    public class Carrot : Enemy
    {
        private CarrotStateMachine _stateMachine;
        [HideInInspector] public bool FacingLeft;
        [HideInInspector]
        public Animator Anim;

        [HideInInspector] public Vector3 PlayerPosition;

        public float DetectionDistance;

        void Start()
        {
            FacingLeft = true;
            Anim = GetComponent<Animator>();
            _stateMachine = new CarrotStateMachine(this);
        }

        public override void FixedUpdate()
        {
            _stateMachine.UpdateState();
        }

        public void Flip()
        {
            FacingLeft = !FacingLeft;
            Vector3 theScale = Anim.transform.localScale;
            theScale.x *= -1;
            Anim.transform.localScale = theScale;
        }
    }
}