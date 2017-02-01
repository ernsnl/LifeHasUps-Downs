using System.ComponentModel;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Ghost
{
    public class GhostAttackState : IEnemyState
    {
        private GhostStateMachine _stateMachine;
        private float _timeCounter;

        #region Collision

        public void OnCollisionEnter2D(Collision2D other)
        {
            // Ghost will not collide with any other object expect terrain
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            // Ghost will not collide with any other object expect terrain
        }

        public void OnCollisionStay2D(Collision2D other)
        {
            // Ghost will not collide with any other object expect terrain
        }

        #endregion

        #region Trigger

        public void OnTriggerEnter2D(Collider2D other)
        {
            // No Need To Check
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            // No Need To Check
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            // No Need To Check
        }

        #endregion

        public void ThrowTrigger(GlobalEnums trigg, bool enterExit)
        {

        }

        public void ToIdleState()
        {
            // No Transition
        }

        public void ToDisappearState()
        {
            _stateMachine.CurrentState = _stateMachine.DisappearState;
        }

        public void UpdateState()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _timeCounter += Time.deltaTime;
            Move();
            _stateMachine.Ghost.Anim.SetFloat("Speed", _stateMachine.Ghost.MaximumSpeed);
            var currentColor = _stateMachine.Ghost.Anim.GetComponent<SpriteRenderer>().color;
            if (currentColor.a < 1f)
            {
                currentColor.a += 1f * Time.deltaTime;
                _stateMachine.Ghost.Anim.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
            }
            
        }

        private void Move()
        {
            var ghost = _stateMachine.Ghost;
            var player = GameObject.FindGameObjectWithTag("Player");
            var pos = player.transform.InverseTransformPoint(ghost.transform.position);
            if(pos.x < 0 && !ghost.FacingLeft) ghost.Flip();
            else if(pos.x> 0 && ghost.FacingLeft) ghost.Flip();

            var playerRayCast = Physics2D.Raycast(ghost.transform.position, player.transform.position, Mathf.Infinity);
            ghost.transform.position = Vector2.MoveTowards(ghost.transform.position, player.transform.position,
                ghost.MaximumSpeed*Time.deltaTime);

            ghost.transform.rotation = player.transform.rotation;
            if (ghost.Floating &&  ghost.FloatingTime < _timeCounter)
            {
                _timeCounter = 0f;
                ToDisappearState();
            }
        }

        public GhostAttackState(GhostStateMachine ghostState)
        {
            this._stateMachine = ghostState;
            this._timeCounter = 0f;
        }
    }
}