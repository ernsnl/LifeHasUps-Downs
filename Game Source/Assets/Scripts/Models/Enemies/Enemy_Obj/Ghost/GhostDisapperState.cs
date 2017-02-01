using Assets.Main;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Ghost
{
    public class GhostDisapperState : IEnemyState
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

        public void ToAttackState()
        {
            _stateMachine.CurrentState = _stateMachine.AttackState;
        }


        #region NoTransition

        public void ToAlertState()
        {
            // No Alert State
        }

        public void ToChaseState()
        {
            // No Chase State
        }

        public void ToPatrolState()
        {
            // No Patrol State
        }

        #endregion 

        public void UpdateState()
        {
            var ghost = _stateMachine.Ghost;
            var player = GameObject.FindGameObjectWithTag("Player");
            _timeCounter += Time.deltaTime;
            ghost.transform.position = new Vector3(-1000, -1000);
            if (_timeCounter > ghost.DisappearTime)
            {
                var direction = GameHandler.Game.Random.Range(0, 1);
                var revealPosition = (direction > 0 ? 1 : -1) * player.transform.right * ghost.RevealDistance + player.transform.position;
                var currentColor = _stateMachine.Ghost.Anim.GetComponent<SpriteRenderer>().color;
                _stateMachine.Ghost.Anim.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f);
                ghost.transform.position = revealPosition;
                ghost.Floating = true;
                _timeCounter = 0f;
                ToAttackState();
            }
        }

        public GhostDisapperState(GhostStateMachine ghostState)
        {
            this._stateMachine = ghostState;
            this._timeCounter = 0f;
        }
    }
}