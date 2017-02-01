using System;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Tank
{
    public class TankChaseState : IEnemyState
    {
        private TankStateMachine _stateMachine;
        private Vector3 _oldPosition;
        private bool _halt;
        private Vector2 _playerPoint;

        #region Collision

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Obstacle")
            {
                _halt = true;
            }
        }

        public void OnCollisionExit2D(Collision2D other)
        {

        }

        public void OnCollisionStay2D(Collision2D other)
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

        public void ToAlertState()
        {
            _stateMachine.CurrentState = _stateMachine.AlertState;
        }

        public void ToAttackState()
        {
            _stateMachine.CurrentState = _stateMachine.AttackState;
        }

        public void UpdateState()
        {
            CastRay();
            Chase();
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
                    tank.LastKnownCollision = castRightRay.point;
                    _playerPoint = castRightRay.point;
                    if (tank.FacingLeft)
                    {
                        tank.Flip();
                        _halt = false;
                        notFound = false;
                    }
                    if (castRightRay.distance < tank.AttackDistance - tank.ChaseBufferDistance)
                        ToAttackState();
                }
            }
            if (castLeftRay)
            {
                if (castLeftRay.collider.tag == "Player")
                {
                    tank.LastKnownCollision = castRightRay.point;
                    _playerPoint = castLeftRay.point;
                    if (!tank.FacingLeft)
                    {
                        tank.Flip();
                        _halt = false;
                        notFound = false;
                    }
                    if (castLeftRay.distance < tank.AttackDistance - tank.ChaseBufferDistance)
                        ToAttackState();
                }       
            }

            if (notFound)
            {
                ToAlertState();
            }
        }
        public void Chase()
        {
            var tank = _stateMachine.Tank;
            var distanceBetween = Vector3.Distance(tank.transform.position, _playerPoint);
            if (!_halt)
            {
                if (distanceBetween > tank.AttackDistance - tank.ChaseBufferDistance)
                    tank.transform.position = Vector3.MoveTowards(tank.transform.position, _playerPoint,
                        tank.MaxSpeed * Time.deltaTime);
                else
                {
                    ToAttackState();
                }
            }
        }

        public TankChaseState(TankStateMachine tank)
        {
            _stateMachine = tank;
            _oldPosition = tank.Tank.transform.position;
            _halt = false;
            _playerPoint = Vector2.zero;
        }
    }
}