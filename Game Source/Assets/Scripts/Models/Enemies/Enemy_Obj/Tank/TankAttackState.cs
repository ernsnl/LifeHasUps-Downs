using System;
using System.Threading;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.Bullets;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Tank
{
    public class TankAttackState : IEnemyState
    {

        private TankStateMachine _stateMachine;
        private Vector3 _oldPosition;
        private bool _cannotFire;
        private float _timer;
        private bool _halt;

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

        #region Trigger

        public void OnTriggerEnter2D(Collider2D other)
        {
            // Player it is already in trigger zone. Therefore no need to check.
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
            if (trigg == Misc.Enums.GlobalEnums.CurvePoints)
            {
                _halt = enterExit;
            }
        }

        public void ToChaseState()
        {
            _stateMachine.CurrentState = _stateMachine.ChaseState;
        }

        public void ToAlertState()
        {
            _stateMachine.CurrentState = _stateMachine.AlertState;
        }

        public void UpdateState()
        {
            _timer += Time.deltaTime;
            CastRay();
            Fire();
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
            bool notFound = true;
            if (castRightRay)
            {
                if (castRightRay.collider.tag == "Player")
                {
                    notFound = false;
                    tank.LastKnownCollision = castRightRay.point;
                    if (tank.FacingLeft)
                        tank.Flip();
                    if (castRightRay.distance > tank.AttackDistance)
                        ToChaseState();
                }
            }
            if (castLeftRay)
            {
                if (castLeftRay.collider.tag == "Player")
                {
                    notFound = false;
                    tank.LastKnownCollision = castLeftRay.point;
                    if (!tank.FacingLeft)
                        tank.Flip();
                    if (castLeftRay.distance > tank.AttackDistance)
                        ToChaseState();
                }
            }
            if (notFound)
            {
                ToAlertState();
            }
        }

        private void Fire()
        {
            if (_timer > _stateMachine.Tank.AttackTimer)
            {           
                _timer = 0f;
                _stateMachine.Tank.Fire();
            }         
        }

        public TankAttackState(TankStateMachine tank)
        {
            _stateMachine = tank;
            _oldPosition = tank.Tank.transform.position;
            _timer = 0f;
            _halt = false;
        }
    }
}