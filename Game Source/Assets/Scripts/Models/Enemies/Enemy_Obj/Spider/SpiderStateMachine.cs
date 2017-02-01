using System;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Spider
{
    public class SpiderStateMachine : MonoBehaviour
    {
        public Spider Spider;

        public IEnemyState CurrentState;
        public IEnemyState SpiderIdleState;
        public IEnemyState SpiderAttackState;

        #region Trigger

        public void OnTriggerExit2D(Collider2D other)
        {
            CurrentState.OnTriggerExit2D(other);
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            CurrentState.OnTriggerStay2D(other);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            CurrentState.OnTriggerEnter2D(other);
        }

        #endregion

        #region Collision 

        public void OnCollisionEnter2D(Collision2D other)
        {
            CurrentState.OnCollisionEnter2D(other);
        }

        public void OnCollisionStay2D(Collision2D other)
        {
            CurrentState.OnCollisionStay2D(other);
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            CurrentState.OnCollisionExit2D(other);
        }

        #endregion

        public void ThrowTrigger(GlobalEnums trigg,bool enterExit)
        {
            CurrentState.ThrowTrigger(trigg,  enterExit);
        }

        public void UpdateState()
        {
            CurrentState.UpdateState();
        }

        public SpiderStateMachine(Spider spider)
        {
            Spider = spider;
            SpiderIdleState = new SpiderIdleState(this);
            SpiderAttackState = new SpiderAttackState(this);
            CurrentState = SpiderIdleState;
        }
    }
}