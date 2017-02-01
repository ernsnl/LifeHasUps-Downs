using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.Enemies.Enemy_Obj.Tank;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Ghost
{
    public class GhostIdleState : IEnemyState
    {
        private GhostStateMachine _stateMachine;

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
        }

        public void OnTriggerExit2D(Collider2D other)
        {
        }

        public void OnTriggerStay2D(Collider2D other)
        {
        }

        #endregion

        public void ThrowTrigger(GlobalEnums trigg, bool enterExit)
        {
            
        }

        public void ToAttackState()
        {
            _stateMachine.CurrentState = _stateMachine.AttackState;
        }

        public void UpdateState()
        {
            CastRay();
            _stateMachine.Ghost.Anim.SetFloat("Speed", 0f);
        }

        public void CastRay()
        {
            var ghost = _stateMachine.Ghost;
            var raycastLocation = ghost.transform.FindChild("RayCast");
            RaycastHit2D hit;
            for (int i = 0; i < 360; i += 15)
            {
                raycastLocation.localRotation = new Quaternion(0, 0, 0, 0);
                raycastLocation.localRotation = Quaternion.AngleAxis(i, raycastLocation.forward);
                var targetPos = raycastLocation.transform.up * ghost.DetectionDistance + raycastLocation.position;
                hit = Physics2D.Linecast(ghost.transform.position, targetPos, 1 << LayerMask.NameToLayer("Player"));
                if (hit)
                {
                    if (hit.transform.tag == "Player")
                    {
                        ToAttackState();
                        ghost.FacingLeft = !ghost.FacingLeft;
                        return;
                    }
                }
                //Debug.DrawRay(raycastLocation.position, raycastLocation.transform.up, Color.green);
            }
        }

        public GhostIdleState(GhostStateMachine ghostState)
        {
            this._stateMachine = ghostState;
        }
    }
}