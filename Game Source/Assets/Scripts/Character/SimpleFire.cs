using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.Bullets;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class SimpleFire : MonoBehaviour
    {
        [HideInInspector]
        public float timer = 0f;
        
        public float _distance = 0.6f;
        public float timeBetweenBullets = 0.15f;

        [HideInInspector]
        public Transform FirePointer;
        [HideInInspector]
        public Transform FireAnchor;
        [HideInInspector]
        public GameObject FireBullet;
        public float FiringForce;
        public float BulletSpeed;
        public float BulletDamage;

        public void Start()
        {
            FirePointer = transform.FindChild("FirePointer");
            FireAnchor = transform.FindChild("Fire_Anchor");
            FireBullet = Resources.Load<GameObject>("Prefabs/Fire/Char_Fire");
        }

        public static float GetFireDamage()
        {
           return  GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleFire>().BulletDamage;
        }

        public void DamageChange(float amount)
        {
            BulletDamage += amount;
        }

        public void SpeedChange(float amount)
        {
            BulletSpeed += amount;
        }

        public void FixedUpdate()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3 dir = FireAnchor.transform.InverseTransformPoint(mousePos);

            var angleBtwn = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            var relativePosition = FireAnchor.transform.localPosition +
                                   new Vector3(Mathf.Cos(angleBtwn * Mathf.Deg2Rad) * _distance, Mathf.Sin(angleBtwn * Mathf.Deg2Rad) * _distance, 0);

            FirePointer.transform.localPosition = new Vector3(relativePosition.x, relativePosition.y, 1f);

            if (transform.up == Vector3.up)
                FirePointer.transform.rotation = Quaternion.Euler(0, 0, angleBtwn);
            else if (transform.up == Vector3.right)
                FirePointer.transform.rotation = Quaternion.Euler(0, 0, angleBtwn - 90);
            else if (transform.up == Vector3.left)
                FirePointer.transform.rotation = Quaternion.Euler(0, 0, angleBtwn - 270);
            else
                FirePointer.transform.rotation = Quaternion.Euler(0, 0, angleBtwn + 180);

            timer += Time.deltaTime;

            if (Input.GetButtonDown("Fire1") && timer >= timeBetweenBullets)
            {
                timer = 0f;
                Instantiate(FireBullet, FirePointer.transform.position, FirePointer.transform.rotation, transform);
            }
        }

    }
}