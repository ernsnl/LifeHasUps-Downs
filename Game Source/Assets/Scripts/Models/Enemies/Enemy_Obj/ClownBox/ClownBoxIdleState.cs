using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.ClownBox
{
    public class ClownBoxIdleState : IEnemyState
    {
        private ClownBoxStateMachine _stateMachine;
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

        private void ToCloseState()
        {
            _stateMachine.CurrentState = _stateMachine.CloseState;
        }

        public void UpdateState()
        {
            CastRay();
        }

        private void CastRay()
        {
            var clownBox = _stateMachine.ClownBox;
            var enemyRightFound = Physics2D.Raycast(clownBox.transform.position, clownBox.transform.right, clownBox.DetectionDistance,
                1 << LayerMask.NameToLayer("Player"));
            var enemyLeftFound = Physics2D.Raycast(clownBox.transform.position, -1 * clownBox.transform.right, clownBox.DetectionDistance,
                1 << LayerMask.NameToLayer("Player"));


            if (enemyLeftFound || enemyRightFound)
            {
                clownBox.Anim.SetBool("EnemyNear", true);
                ToCloseState();
            }
        }

        public ClownBoxIdleState(ClownBoxStateMachine clownBoxState)
        {
            _stateMachine = clownBoxState;
            _timeCounter = 0f;
        }

    }
}