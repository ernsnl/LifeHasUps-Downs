using System;
using Assets.Main;
using Assets.Scripts.Misc.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        public float BaseHealth;
        public float OffSet = 0f;
        public float MaxSpeed = 0f;
        public float MaximumPatrolDistance = 0f;
        public float MovementForce = 0f;
        public bool CanMove = true;
        public Transform PlayerLocation;
        public float AttackDistance = 2f;
        public float AttackTimer = 0.75f;
        public GameObject Bullet;
        public Transform FirePointer;

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void OnCollisionEnter2D(Collision2D coll)
        {
        }

        public virtual void OnTriggerEnter2D(Collider2D coll)
        {
        }

        public virtual void OnTriggerStay2D(Collider2D coll)
        {
        }

        public virtual void OnTriggerExit2D(Collider2D coll)
        {
        }

        public virtual void OnCollisionStay2D(Collision2D coll)
        {
            
        }

        public virtual void OnCollisionExit2D(Collision2D coll)
        {
            
        }

        public virtual void ThrowTrigger(GlobalEnums trig, bool enterExit)
        {
            
        }

        public void OnDestroy()
        {
            var ExitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
            if (ExitDoor)
            {
                var ExitDoorInfo = ExitDoor.GetComponent<ExitRoom.ExitRoom>();
                ExitDoorInfo.KilledEnemy();
            }
        }
    }
}