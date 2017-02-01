using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class SimpleMovement : MonoBehaviour
    {
        public float moveForce = 40f;
        public float maxSpeed = 4f;
        public float jumpForce = 25f;
        public float offSetY = 0.06f;
        public GameObject Anim;

        //Hidden In Inspector
        private Rigidbody2D rb2d;
        private Animator _anim;

        //Neccessary For Calculation
        public Transform groundCheck;

        //Internal calculation properties
        private bool jump = false;
        private bool grounded = true;
        private bool onTopOfObstacles = false;
        private bool facingRight;

        //GravityManupulation
        private const int gravity = -30;
        private Vector3 playerNormal;

        //Rotation Manupulation
        private const int rayDistance = 1;
        private Vector3 upTransform;

        private Vector3 surfaceNormal;

        public Transform leftFoot;
        public Transform rightFoot;
        public Transform middle;

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.freezeRotation = true;
            playerNormal = transform.up;
            upTransform = transform.up;
            _anim = Anim.GetComponent<Animator>();
            facingRight = true;
        }   

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 theScale = Anim.transform.localScale;
            theScale.x *= -1;
            Anim.transform.localScale = theScale;
        }

        private void MousePosition()
        {
            Vector3 objectPos = transform.InverseTransformPoint(this.transform.FindChild("FirePointer").transform.position);
            if (objectPos.x < 0 && facingRight)
            {
                Flip();
            }
            else if (objectPos.x > 0 && !facingRight)
            {
                Flip();
            }
        }

        private void HandleJump()
        {
            if (!jump)
                _anim.SetFloat("Speed", rb2d.velocity.magnitude);
            _anim.SetBool("Grounded", !jump);

            if (jump)
            {
                transform.position = transform.position + transform.up * 0.2f;
                if (transform.up == Vector3.right || transform.up == Vector3.left)
                {
                    rb2d.AddForce(transform.up * jumpForce);
                    rb2d.AddForce(transform.up * gravity * -1);
                }
                else
                {
                    rb2d.AddForce(transform.up * jumpForce);
                }

                jump = false;
            }

            if (Input.GetButtonDown("Jump") && (grounded || onTopOfObstacles))
            {
                onTopOfObstacles = false;
                grounded = false;
                jump = true;
            }
        }

        private void DrawGameObjectName(Transform transform, GizmoType gizmoType)
        {
            Handles.Label(transform.position, transform.gameObject.name);
        }

        private void HandleSpeed(float direction)
        {
            if (direction > 0f || direction < 0f)
            {
                if (rb2d.velocity.magnitude < maxSpeed)
                {
                    rb2d.AddForce(transform.right*direction*moveForce);
                }

                if (Mathf.Abs(rb2d.velocity.x) > maxSpeed && grounded)
                {
                    rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x)*maxSpeed, rb2d.velocity.y);
                }
            }
        }

        private void HandleRotation()
        {
            if (grounded)
            {

                Debug.DrawRay(transform.position, transform.up, Color.red, 0.01f);

                surfaceNormal = transform.up;
                var rayCastLeft = Physics2D.Raycast(leftFoot.position, -1 * transform.up, rayDistance,
                    1 << LayerMask.NameToLayer("TerrainLayerMask"));
                var rayCastRight = Physics2D.Raycast(rightFoot.position, -1 * transform.up, rayDistance,
                    1 << LayerMask.NameToLayer("TerrainLayerMask"));
                var rayCastMiddle = Physics2D.Raycast(middle.position, -1 * transform.up, rayDistance,
                    1 << LayerMask.NameToLayer("TerrainLayerMask"));

                if (rayCastMiddle.collider != null && !jump)
                {
                    Debug.DrawRay(rayCastRight.point, rayCastRight.normal, Color.cyan, 0.01f);
                    Debug.DrawRay(rayCastLeft.point, rayCastLeft.normal, Color.yellow, 0.01f);
                    Debug.DrawRay(rayCastMiddle.point, rayCastMiddle.normal, Color.green, 0.01f);

                    Vector3 avrNormal = (rayCastRight.normal + rayCastLeft.normal + rayCastMiddle.normal) / 3;
                    Vector3 averagePoint = (rayCastLeft.point + rayCastMiddle.point + rayCastRight.point) / 3;
                    surfaceNormal = avrNormal;

                    Quaternion targetRotation = Quaternion.FromToRotation(playerNormal, surfaceNormal);
                    Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5);

                    //transform.rotation = Quaternion.Slerp(targetRotation, transform.rotation , 50 * Time.deltaTime);

                    transform.rotation = Quaternion.Euler(0, 0, finalRotation.eulerAngles.z);
                    transform.position = averagePoint + transform.up * offSetY;
                }
            }

        }

        private void FixedUpdate()
        {

            // Grounded Check
            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("TerrainLayerMask"));
            onTopOfObstacles = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Obstacles"));

            //Contstant Gravity to Object. It has to be applied no matter what
            rb2d.AddForce(transform.up * gravity);

            // Movement Direction
            float h = Input.GetAxis("Horizontal");

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

            MousePosition();
            HandleSpeed(h);
            HandleJump();
            HandleRotation();
        }

        public void ChangeSpeed(float speedModifier)
        {
            //Debug.Log("MaxSpeed: " + maxSpeed);
            maxSpeed += speedModifier;
            //Debug.Log("MaxSpeed: " + maxSpeed);
        }
    }
}