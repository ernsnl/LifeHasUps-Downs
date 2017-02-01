using System;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Tank
{
    public class TankAlertState : IEnemyState
    {

        private TankStateMachine _stateMachine;
        private bool _cannotGo;
        private float _timeCounter;

        #region Collision

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Obstacle")
            {
                _cannotGo = true;
            }
        }

        public void OnCollisionExit2D(Collision2D other)
        {

        }

        public void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.tag == "Obstacle")
            {
                _cannotGo = true;
            }
        }

        #endregion

        #region Trigger

        public void OnTriggerEnter2D(Collider2D other)
        {

        }

        public void OnTriggerExit2D(Collider2D other)
        {
            // Player Cannot exit in alert state
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            // Player Cannot Stay in alert state
        }

        #endregion

        public void ThrowTrigger(GlobalEnums trigg, bool enterExit)
        {
            if (trigg == Misc.Enums.GlobalEnums.CurvePoints)
            {
                _cannotGo = enterExit;
            }
        }

        public void ToAttackState()
        {
            _stateMachine.CurrentState = _stateMachine.AttackState;
        }

        public void ToChaseState()
        {
            _stateMachine.CurrentState = _stateMachine.ChaseState;
        }

        public void ToPatrolState()
        {
            _stateMachine.CurrentState = _stateMachine.PatrolState;
        }

        public void UpdateState()
        {
            var tank = _stateMachine.Tank;

            if (_timeCounter < tank.AlertStateTimer)
            {
                CastRay();
                Alert();
            }
            else
            {
                _timeCounter = 0f;
                ToPatrolState();
            }
            _timeCounter += Time.deltaTime;
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
                    tank.LastKnownCollision = castRightRay.point;
                    if (tank.FacingLeft)
                    {
                        tank.Flip();
                        _cannotGo = false;
                    }
                    if (castRightRay.distance > tank.AttackDistance - tank.ChaseBufferDistance)
                        ToChaseState();
                    if (castRightRay.distance < tank.AttackDistance - tank.ChaseBufferDistance)
                        ToAttackState();
                }
            }
            if (castLeftRay)
            {
                if (castLeftRay.collider.tag == "Player")
                {
                    tank.LastKnownCollision = castRightRay.point;
                    if (!tank.FacingLeft)
                    {
                        tank.Flip();
                        _cannotGo = false;
                    }
                    if (castLeftRay.distance > tank.AttackDistance - tank.ChaseBufferDistance)
                        ToChaseState();
                    if (castLeftRay.distance < tank.AttackDistance - tank.ChaseBufferDistance)
                        ToAttackState();
                }
            }

        }

        private void Alert()
        {

            var tank = _stateMachine.Tank;
            var direction = tank.FacingLeft ? -1 : 1;
            var distanceBetween = Vector3.Distance(tank.transform.position, tank.LastKnownCollision);
            if (distanceBetween > 0.5f)
                tank.transform.position = Vector3.MoveTowards(tank.transform.position, tank.LastKnownCollision, tank.MaxSpeed * Time.deltaTime);
            else
            {
                tank.Flip();
            }
        }
        
        public TankAlertState(TankStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _cannotGo = false;
            _timeCounter = 0f;
        }
    }
}