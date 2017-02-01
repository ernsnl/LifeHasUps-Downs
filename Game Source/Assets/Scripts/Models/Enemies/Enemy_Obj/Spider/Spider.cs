using Assets.Main;
using Assets.Scripts.Character;
using Assets.Scripts.Misc.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Spider
{
    public class Spider : Enemy
    {
        private SpiderStateMachine _stateMachine;
        [HideInInspector]
        public  GameObject SpiderString;

        public int VisibleArc;
        public float VisibleDistance;
        public float SpiderAttackSpeed;

        void Start()
        {
            SpiderString = Resources.Load<GameObject>("Prefabs/Misc/Spider_String");
            _stateMachine = new SpiderStateMachine(this);

            var spiderBase = transform.FindChild("Spider_Base");
            var spiderConnection = transform.FindChild("Spider_Connection");
            
            var spiderLineRenderer = GetComponent<LineRenderer>();
            spiderLineRenderer.SetPosition(0, spiderBase.position);
            spiderLineRenderer.SetPosition(1, spiderConnection.position);

            var castRayDown = Physics2D.Raycast(transform.position, transform.up, GlobalConst.BigNumber,
               1 << LayerMask.NameToLayer("TerrainLayerMask"));
            if (castRayDown)
            {              
                var distance = castRayDown.point - (Vector2)transform.position;
                
                spiderBase.transform.position = spiderBase.transform.position + transform.up*distance.magnitude/4;
            }
          
           
        }


        public override void FixedUpdate()
        {
            var spiderBase = transform.FindChild("Spider_Base");
            var spiderConnection = transform.FindChild("Spider_Connection");

            var spiderLineRenderer = GetComponent<LineRenderer>();
            spiderLineRenderer.SetPosition(0, spiderBase.position);
            spiderLineRenderer.SetPosition(1, spiderConnection.position);

            _stateMachine.UpdateState();
        }


        public override void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player_Bullet")
            {
                BaseHealth -= SimpleFire.GetFireDamage();
                if (BaseHealth < 0f)
                {
                    GameHandler.Game.Spawn.CreatePickUp(this.transform);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}