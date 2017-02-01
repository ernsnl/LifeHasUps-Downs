using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Tank
{
    public class TankPatrolState : IEnemyState
    {
        private TankStateMachine _stateMachine;
        private float _distanceTraveled;
        private bool _slowDown = false;
        private float _stuckTime;
        private bool _enemyFound;
        private Vector2 _patrolPoint;

        #region Trigger

        public void OnTriggerEnter2D(Collider2D other)
        {
           
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            // Unnecessary Trigger Event For This Enemy, It can be used for much sophisticated bahaviour.
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            // Unnecessary Trigger Event For This Enemy, It can be used for much sophisticated bahaviour.
        }

        #endregion

        #region Collision

        public void OnCollisionEnter2D(Collision2D other)
        {

        }

        public void OnCollisionStay2D(Collision2D other)
        {
            // Unnecessary Collision Event For This Enemy, It can be used for much sophisticated bahaviour.
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            // Unnecessary Collision Event For This Enemy, It can be used for much sophisticated bahaviour.
        }

        #endregion



        public void ThrowTrigger(GlobalEnums trigg, bool enterExit)
        {
            if (trigg == Misc.Enums.GlobalEnums.CurvePoints)
            {
                if (enterExit)
                {
                    _stateMachine.Tank.Flip();
                    _patrolPoint = Vector2.zero;
                }
            }           
        }

        #region StateTransition

        public void ToAttackState()
        {
            _enemyFound = false;
            _stateMachine.CurrentState = _stateMachine.AttackState;
        }

        #endregion

        public void UpdateState()
        {
            CastRay();
            Patrol();
        }

        private void CastRay()
        {           
            var tank = _stateMachine.Tank;
            var castRightRay = Physics2D.Raycast(tank.transform.position, tank.transform.right, tank.RayDistance,
                1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Obstacle") |
                1 << LayerMask.NameToLayer("TerrainLayerMask"));
            var castLeftRay = Physics2D.Raycast(tank.transform.position, tank.transform.right * -1, tank.RayDistance,
                1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Obstacle") |
                1 << LayerMask.NameToLayer("TerrainLayerMask"));
            if (castRightRay)
            {
                if (castRightRay.collider.tag == "Player")
                {
                    _enemyFound = true;
                    if(tank.FacingLeft)
                        tank.Flip();
                    tank.LastKnownCollision = castRightRay.collider.transform.position;
                    ToAttackState();
                }
                else if (castRightRay.collider.tag == "Obstacle")
                {
                    if (castRightRay.distance < 0.5f)
                    {
                        _patrolPoint = Vector2.zero;
                        tank.Flip();
                    }
                }
            }
             if(castLeftRay)
            {
                if (castLeftRay.collider.tag == "Player")
                {
                    _enemyFound = true;
                    if (!tank.FacingLeft)
                        tank.Flip();
                    tank.LastKnownCollision = castLeftRay.collider.transform.position;
                    ToAttackState();
                }
                else if (castLeftRay.collider.tag == "Obstacle")
                {
                    if (castLeftRay.distance < 0.5f)
                    {
                        _patrolPoint = Vector2.zero;
                        tank.Flip();
                    }
                }
            }
        }

        private void Patrol()
        {
            if (!_enemyFound)
            {
                var tank = _stateMachine.Tank;
                var direction = tank.FacingLeft ? -1 : 1;
                var patrolDistance = new Vector3(tank.MaximumPatrolDistance, tank.MaximumPatrolDistance);
                patrolDistance.Scale(tank.transform.right);
                patrolDistance *= direction;
                if(Vector2.zero == _patrolPoint)
                    _patrolPoint = tank.transform.position + patrolDistance;
                var distanceBetween = Vector3.Distance(tank.transform.position, _patrolPoint);
                if (distanceBetween > 0.5f)
                    tank.transform.position = Vector3.MoveTowards(tank.transform.position, _patrolPoint, tank.MaxSpeed*Time.deltaTime);
                else
                {
                    patrolDistance *= -1;
                    tank.Flip();
                    _patrolPoint = tank.transform.position + patrolDistance;
                    tank.transform.position = Vector3.MoveTowards(tank.transform.position, _patrolPoint, tank.MaxSpeed * Time.deltaTime);
                }
            }
        }

        public TankPatrolState(TankStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _enemyFound = false;
            _patrolPoint = Vector2.zero;
        }
    }
}