using System;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Carrot
{
    public class CarrotAttackState : IEnemyState
    {
        private CarrotStateMachine _stateMachine;

        public CarrotAttackState(CarrotStateMachine stateMachine)
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

        public void ThrowTrigger(GlobalEnums trigg, bool enterExit)
        {

        }

       

        public void ToIdleState()
        {
           
        }

        public void UpdateState()
        {
            CalculateMovement();
        }

        private void CalculateMovement()
        {
            _stateMachine.Carrot.Anim.SetBool("Rolling", true);
            _stateMachine.Carrot.Anim.SetFloat("Speed", _stateMachine.Carrot.MaxSpeed);
            _stateMachine.Carrot.transform.position = Vector3.MoveTowards(_stateMachine.Carrot.transform.position, _stateMachine.Carrot.PlayerPosition,
                _stateMachine.Carrot.MaxSpeed*Time.deltaTime);
        }
    }
}