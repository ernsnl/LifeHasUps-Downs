using System.Xml.Schema;
using Assets.Main;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.Clown
{
    public class Clown : Enemy
    {

        private Rigidbody2D _rgb2D;
        public bool _facingLeft;
        private Transform _leftFoot;
        private Transform _middle;
        private Transform _rightFoot;
        private Vector3 playerNormal;
        private Animator _anim;
        private bool _stun;

        public float StunTimer = 2f;

        private float _timeCounter;
        private int _flipCounter;

        public void Start()
        {
            _rgb2D = GetComponent<Rigidbody2D>();
            _leftFoot = transform.FindChild("LeftFoot");
            _middle = transform.FindChild("Middle");
            _rightFoot = transform.FindChild("RightFoot");
            _anim = GetComponent<Animator>();
            playerNormal = Vector3.up;
            _flipCounter = 1;
            _facingLeft = transform.parent.GetComponent<ClownBox.ClownBox>()._facingLeft;
            if (!_facingLeft)
            {
                Flip();
                _facingLeft = false;
                
            }
            transform.parent = null;
        }

        

        public void Flip()
        {
            _facingLeft = !_facingLeft;
            Vector3 theScale = _anim.transform.localScale;
            theScale.x *= -1;
            _anim.transform.localScale = theScale;
        }

        private void AdjustRotation()
        {
            var direction = _facingLeft ? -1 : 1;

            if (_rgb2D.velocity.magnitude < MaxSpeed)
            {
                _rgb2D.AddForce(transform.right * direction * MovementForce);
            }

            //Debug.DrawRay(transform.position, transform.up, Color.red, 0.5f);

            var rayCastLeft = Physics2D.Raycast(_leftFoot.position, -1 * transform.up, 1,
                1 << LayerMask.NameToLayer("TerrainLayerMask"));
            var rayCastRight = Physics2D.Raycast(_rightFoot.position, -1 * transform.up, 1,
                1 << LayerMask.NameToLayer("TerrainLayerMask"));
            var rayCastMiddle = Physics2D.Raycast(_middle.position, -1 * transform.up, 1,
                1 << LayerMask.NameToLayer("TerrainLayerMask"));

            //Debug.DrawRay(transform.position, transform.up, Color.red, 0.5f);

            if (rayCastMiddle.collider != null)
            {
                var surfaceNormal = transform.up;
                //Debug.DrawRay(rayCastRight.point, rayCastRight.normal, Color.cyan, 0.5f);
                //Debug.DrawRay(rayCastLeft.point, rayCastLeft.normal, Color.yellow, 0.5f);
                //Debug.DrawRay(rayCastMiddle.point, rayCastMiddle.normal, Color.green, 0.5f);

                Vector3 avrNormal = (rayCastRight.normal + rayCastLeft.normal + rayCastMiddle.normal) / 3;
                Vector3 averagePoint = (rayCastLeft.point + rayCastMiddle.point + rayCastRight.point) / 3;
                surfaceNormal = avrNormal;


                Quaternion targetRotation = Quaternion.FromToRotation(playerNormal, surfaceNormal);

                Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5);

                //transform.rotation = Quaternion.Slerp(targetRotation, transform.rotation , 50 * Time.deltaTime);

                transform.rotation = Quaternion.Euler(0, 0, finalRotation.eulerAngles.z);
                transform.position = averagePoint + transform.up * OffSet;

            }
        }

        public override void FixedUpdate()
        {
            _rgb2D.AddForce(transform.up * Physics2D.gravity.magnitude * -1);

            if (!_stun)
            {
                var groundLeftCheck = transform.FindChild("groundLeftCheck");
                var groundedLeft = Physics2D.Linecast(transform.position, groundLeftCheck.position, 1 << LayerMask.NameToLayer("TerrainLayerMask"));
                var groundRightCheck = transform.FindChild("groundRightCheck");
                var groundedRight = Physics2D.Linecast(transform.position, groundRightCheck.position, 1 << LayerMask.NameToLayer("TerrainLayerMask"));
                AdjustRotation();

            }
            else
            {
                _timeCounter += Time.deltaTime;
                if (_timeCounter > StunTimer)
                {
                    _timeCounter = 0f;
                    _stun = false;
                    Flip();
                }
            }

            _anim.SetFloat("Speed", _rgb2D.velocity.magnitude);
        }

        public override void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Obstacle")
            {
                _stun = true;
            }

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

        public override void OnCollisionStay2D(Collision2D coll)
        {

        }


    }
}