using System;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Carrot
{
    public class CarrotStateMachine : MonoBehaviour
    {
        public Carrot Carrot;

        public IEnemyState CurrentState;
        public IEnemyState IdleState;
        public IEnemyState AttackState;

        #region Collision

        public void OnCollisionEnter2D(Collision2D other)
        {
           
        }

        public void OnCollisionExit2D(Collision2D other)
        {
           
        }

        public void OnCollisionStay2D(Collision2D other)
        {
           
        }

        #endregion

        // No Trigger

        #region Trigger

        public void OnTriggerEnter2D(Collider2D other)
        {
            
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            ;
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            
        }

        public void ThrowTrigger(GlobalEnums trigg)
        {
            
        }
        #endregion


        public void UpdateState()
        {
            CurrentState.UpdateState();
        }

        public CarrotStateMachine(Carrot carrot)
        {
            Carrot = carrot;
            IdleState = new CarrotIdleState(this);
            AttackState = new CarrotAttackState(this);
            CurrentState = IdleState;
        }
    }
}