using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.ClownBox
{
    public class ClownBoxAttackState : IEnemyState
    {
        private ClownBoxStateMachine _stateMachine;
        private float _timeCounter;
        private bool flip;

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
            _stateMachine.CurrentState = _stateMachine.IdleState;
        }

        public void UpdateState()
        {
            _timeCounter += Time.deltaTime;
            var clownBox = _stateMachine.ClownBox;
            CastRay();
            SpawnClownHead();
        }

        private void SpawnClownHead()
        {
            var clownBox = _stateMachine.ClownBox;

            if (clownBox.AttackTimer < _timeCounter)
            {
                var clownSpawnPoint = clownBox.transform.FindChild("ClownHeadPoint");
                var clownHead = (GameObject)Resources.Load("Prefabs/Enemy/ClownHead");
                var clownHeadInfo = clownHead.GetComponent<Enemy>();
                Vector3 pos = new Vector3(clownSpawnPoint.position.x, clownSpawnPoint.position.y, 0);
                Vector3 offSet = new Vector3(clownHeadInfo.OffSet, clownHeadInfo.OffSet, 0);
                offSet.Scale(clownSpawnPoint.up);
                pos += offSet;
                var copy = (GameObject)GameObject.Instantiate(clownHead, pos, clownSpawnPoint.transform.rotation);
                copy.transform.SetParent(clownBox.transform);
                _timeCounter = 0f;
            }
        }

        private void CastRay()
        {
            var clownBox = _stateMachine.ClownBox;
            var enemyRightFound = Physics2D.Raycast(clownBox.transform.position, clownBox.transform.right, clownBox.DetectionDistance,
                1 << LayerMask.NameToLayer("Player"));
            var enemyLeftFound = Physics2D.Raycast(clownBox.transform.position, -1 * clownBox.transform.right, clownBox.DetectionDistance,
                1 << LayerMask.NameToLayer("Player"));

            if (enemyLeftFound && !clownBox._facingLeft)
            {
                clownBox.Flip();
                flip = false;
            }
            else if (enemyRightFound && clownBox._facingLeft)
            {
                clownBox.Flip();
                flip = true;
            }

            if (!enemyLeftFound && !enemyRightFound)
            {
                clownBox.Anim.SetBool("EnemyNear", false);
                ToIdleState();
            }

        }

        public ClownBoxAttackState(ClownBoxStateMachine clownState)
        {
            this._stateMachine = clownState;
            this._timeCounter = 0f;
            flip = false;
        }
    }
}