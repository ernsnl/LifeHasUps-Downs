using System;
using Assets.Main;
using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.Enemies.Enemy_Obj.Ghost;
using Assets.Scripts.Models.States;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Spider
{
    public class SpiderIdleState : IEnemyState
    {
        private SpiderStateMachine _stateMachine;
        private float _timeCounter;
        private Vector3 _currentTarget;
        public Vector3 _currentRayCastTarget;

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

        public void ToAttackState()
        {
            _stateMachine.CurrentState = _stateMachine.SpiderAttackState;
        }

        public void UpdateState()
        {
            if (CastRay())
                ToAttackState();
            CreateUpDownMovement();
            _timeCounter += Time.deltaTime;
        }

        public bool CastRay()
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
                hit = Physics2D.Linecast(startPos, targetPos, 1 << LayerMask.NameToLayer("Player"));
                if (hit)
                {
                    return true;
                }
                //Debug.DrawLine(startPos, targetPos, Color.green);
            }
            return false;
        }

        public void CreateUpDownMovement()
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

            if (_timeCounter < 5f && _currentTarget != Vector3.zero)
            {
                spiderBase.transform.position = Vector3.MoveTowards(spiderBase.position, _currentTarget,
                    spider.MaxSpeed * Time.deltaTime);
                spiderRayCast.transform.position = Vector3.MoveTowards(spiderRayCast.position, _currentRayCastTarget,
                    spider.MaxSpeed * Time.deltaTime);
                return;
            }
            _timeCounter = 0f;

            var castRayDown = Physics2D.Raycast(spiderBase.transform.position, spider.transform.up, GlobalConst.BigNumber,
                1 << LayerMask.NameToLayer("TerrainLayerMask"));
            var castRayUp = Physics2D.Raycast(spiderBase.transform.position, spider.transform.up * -1, GlobalConst.BigNumber,
                1 << LayerMask.NameToLayer("TerrainLayerMask"));

            if (castRayDown)
            {
                var randomDirection = GameHandler.Game.Random.Range(0, 1) > 0 ? 1 : -1;
                var distanceDown = castRayDown.point - (Vector2)spiderBase.transform.position;
                var distanceUp = castRayUp.point - (Vector2)spiderBase.transform.position;

                if (randomDirection > 0)
                {
                    _currentTarget = spiderBase.transform.position + spider.transform.up * distanceDown.magnitude / 4;
                    _currentRayCastTarget = spiderRayCast.transform.position + spider.transform.up * distanceDown.magnitude / 4;
                }
                else
                {
                    _currentTarget = spiderBase.transform.position + -1 * spider.transform.up * distanceUp.magnitude / 4;
                    _currentRayCastTarget = spiderRayCast.transform.position + -1 * spider.transform.up * distanceDown.magnitude / 4;
                }
            }



        }

        public SpiderIdleState(SpiderStateMachine spiderState)
        {
            this._stateMachine = spiderState;
            _timeCounter = 0f;
        }
    }
}