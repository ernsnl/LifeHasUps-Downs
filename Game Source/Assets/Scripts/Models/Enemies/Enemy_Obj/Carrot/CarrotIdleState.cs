using System;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Carrot
{
    public class CarrotIdleState : IEnemyState
    {

        private CarrotStateMachine _stateMachine;

        public CarrotIdleState(CarrotStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void OnCollisionEnter2D(Collision2D other)
        {

        }

        public void OnCollisionExit2D(Collision2D other)
        {

        }

        public void OnCollisionStay2D(Collision2D other)
        {

        }

        public void OnTriggerEnter2D(Collider2D other)
        {

        }

        public void OnTriggerExit2D(Collider2D other)
        {

        }

        public void OnTriggerStay2D(Collider2D other)
        {

        }

        public void ThrowTrigger(GlobalEnums trigg,  bool enterExit)
        {

        }

        public void ToAttackState()
        {
            _stateMachine.CurrentState = _stateMachine.AttackState;
        }


        public void UpdateState()
        {
            CastRay();
        }

        private void CastRay()
        {
            var carrot = _stateMachine.Carrot;

            var enemyRightFound = Physics2D.Raycast(carrot.transform.position, carrot.transform.right, carrot.DetectionDistance,
                1 << LayerMask.NameToLayer("Player"));
            var enemyLeftFound = Physics2D.Raycast(carrot.transform.position, -1 * carrot.transform.right, carrot.DetectionDistance,
                1 << LayerMask.NameToLayer("Player"));

            if (enemyLeftFound || enemyRightFound)
            {
                if (enemyLeftFound)
                    _stateMachine.Carrot.PlayerPosition = enemyLeftFound.point;
                if (enemyRightFound)
                    _stateMachine.Carrot.PlayerPosition = enemyRightFound.point;
                //Debug.Log(_stateMachine.Carrot.PlayerPosition);
                ToAttackState();
            }
        }
    }
}