using Assets.Main;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.BeeHive
{
    public class DefenderBeeFlocking : MonoBehaviour
    {
        [HideInInspector]
        public int BeeCount;
        private float _beeDistance = 0.5f;
        private float _beeRotationCounter;
        [HideInInspector]
        public bool IsAggressive;
        public float DefensiveStand = 2.5f;
        public float FlockSpeed = 2.5f;
        private Vector3 _currentPlayerPosition;


        public void Start()
        {
            IsAggressive = false;
            _beeRotationCounter = 0f;
            BeeCount = transform.childCount;
            _currentPlayerPosition = Vector3.zero;
        }

        public void CalculateDistance()
        {
            float angleFormation = (float)360 / (float)transform.childCount;
            var firstBee = transform.GetChild(0);
            var beeColliderBounds = firstBee.GetComponent<CircleCollider2D>();
            var radiusBee = beeColliderBounds.radius;
            var root = Mathf.Pow(radiusBee * 1.5f * 2, 2f) / (2 - 2 * Mathf.Cos(angleFormation * Mathf.Deg2Rad));
            _beeDistance = Mathf.Sqrt(root);


        }

        private void BeeMovement()
        {
            BeeCount = transform.childCount;
            if (BeeCount > 0)
            {
                if (BeeCount == 1)
                {
                    var getBee = transform.GetChild(0);
                    getBee.transform.position = transform.position;
                }
                else
                {
                    float angleFormation = (float)360 / (float)transform.childCount;
                    CalculateDistance();
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        var getBee = transform.GetChild(i);

                        var beeDistanceX = Mathf.Cos(Mathf.Deg2Rad * (angleFormation * i + _beeRotationCounter)) * _beeDistance;
                        var beeDistanceY = Mathf.Sin(Mathf.Deg2Rad * (angleFormation * i + _beeRotationCounter)) * _beeDistance;
                        var beePos = new Vector3(beeDistanceX, beeDistanceY);
                        var currentWorldPosition = transform.TransformPoint(beePos);
                        getBee.transform.position = Vector3.MoveTowards(getBee.transform.position, currentWorldPosition, 5 * Time.deltaTime);

                    }
                }
                _beeRotationCounter += 100 * Time.deltaTime;
            }
        }

        private void FlockMovement()
        {
            if (transform.childCount > 0)
            {
                if (transform.parent != null)
                {
                    var playerDistance = Vector3.Distance(transform.position, transform.parent.position);
                    if (DefensiveStand > playerDistance && _currentPlayerPosition != Vector3.zero)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, _currentPlayerPosition,
                            Time.deltaTime*FlockSpeed);
                    }
                    else
                    {
                        var returnPoint = transform.parent.FindChild("ReturnPoint");
                        transform.position = Vector3.MoveTowards(transform.position, returnPoint.position,
                            Time.deltaTime*FlockSpeed);
                    }
                }
                else
                {
                    var currentPlayer = GameHandler.Game.Player;
                    transform.position = Vector3.MoveTowards(transform.position, currentPlayer.transform.position,
                        Time.deltaTime*FlockSpeed);
                }
            }
        }

        public void FixedUpdate()
        {
            BeeMovement();
            FlockMovement();
        }



        public void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.tag == "Player" && IsAggressive)
            {
                _currentPlayerPosition = coll.transform.position;
            }
        }

        public void OnTriggerStay2D(Collider2D coll)
        {
            if (coll.tag == "Player" && IsAggressive)
            {
                _currentPlayerPosition = coll.transform.position;
            }
        }

        public void OnTriggerExit2D(Collider2D coll)
        {
            if (!coll.isTrigger && coll.tag == "Player" && IsAggressive)
            {
                _currentPlayerPosition = Vector3.zero;
            }
        }
    }
}