using System;
using Assets.Main;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Spider
{
    public class SpiderAttackState : IEnemyState
    {
        private SpiderStateMachine _stateMachine;
        private Vector3 _firstHitLocation;
        private Vector3 _hitLocation;
        private int _hitCounter;
        private float _timeCounter;
        private Vector2 _playerSpeed;
        private float _hitLocationAngle;

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
            _stateMachine.CurrentState = _stateMachine.SpiderIdleState;
        }

        public void UpdateState()
        {
            var spider = _stateMachine.Spider;
            if (_timeCounter > spider.AttackTimer && _timeCounter < 1.5f)
            {
                Attack();
                _hitCounter = 0;
            }
            else
            {
                if (_timeCounter > 1.5f)
                    CastRay();
            }
            _timeCounter += Time.deltaTime;
        }

        private void Attack()
        {


            var spider = _stateMachine.Spider;
            var spiderBase = spider.transform.FindChild("Spider_Base");
            var spiderConnection = spider.transform.FindChild("Spider_Connection");
            var spiderRayCast = spiderBase.transform.FindChild("Spider_RayCast");

            var spiderLineRenderer = spider.GetComponent<LineRenderer>();
            var spiderPos = new Vector3(spiderBase.position.x, spiderBase.position.y, 1);
            var spiderConPos = new Vector3(spiderConnection.position.x, spiderConnection.position.y, 1);
            spiderLineRenderer.SetPosition(0, spiderPos);
            spiderLineRenderer.SetPosition(1, spiderConPos);

            var randomDirection = GameHandler.Game.Random.Range(0, 2);
            var angle = randomDirection > 0
                ?(randomDirection == 1 ? _hitLocationAngle + spider.VisibleArc / 10 : _hitLocationAngle)
                : _hitLocationAngle - spider.VisibleArc / 10;

            spiderRayCast.localRotation = new Quaternion(0, 0, 0, 0);
            spiderRayCast.localRotation = Quaternion.AngleAxis(angle, spiderRayCast.forward);
            var targetPos = spiderRayCast.transform.up * spider.VisibleDistance + spiderRayCast.position;
            spiderBase.transform.position = Vector3.MoveTowards(spiderBase.position, targetPos, Time.deltaTime * spider.SpiderAttackSpeed);


        }

        private void CastRay()
        {
            var spider = _stateMachine.Spider;
            var spiderBase = spider.transform.FindChild("Spider_Base");
            var spiderRayCast = spiderBase.transform.FindChild("Spider_RayCast");
            var startPos = spiderBase.transform.position;
            int increment = spider.VisibleArc / 10;


            RaycastHit2D hit;
            for (int i = -spider.VisibleArc / 2; i < spider.VisibleArc / 2; i += increment)
            {
                spiderRayCast.localRotation = new Quaternion(0, 0, 0, 0);
                spiderRayCast.localRotation = Quaternion.AngleAxis(i, spiderRayCast.forward);
                var targetPos = spiderRayCast.transform.up * spider.VisibleDistance + spiderRayCast.position;
                hit = Physics2D.Linecast(startPos, targetPos, 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("TerrainLayerMask"));
                if (hit)
                {
                    if (hit.transform.tag == "Player")
                    {
                        _hitLocationAngle = i;
                        _hitLocation = hit.point;
                        _playerSpeed = hit.transform.GetComponent<Rigidbody2D>().velocity;
                        _timeCounter = 0f;
                        return;
                    }
                }
                //Debug.DrawLine(startPos, targetPos, Color.yellow);
            }

            ToIdleState();


        }

        public SpiderAttackState(SpiderStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _timeCounter = 0f;
        }
    }
}