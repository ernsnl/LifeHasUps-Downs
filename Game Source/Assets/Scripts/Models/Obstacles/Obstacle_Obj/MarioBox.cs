using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Main;
using Assets.Scripts.Misc.CustomFunctions;
using Assets.Scripts.Models.PickUps;
using UnityEngine;

namespace Assets.Scripts.Models.Obstacles.Obstacle_Obj
{
    public class MarioBox : Obstacle
    {
        private Transform _hitCheck;

        private bool _collisionOccured;
        private float _timeCounter;
        private Animator _anim;
        private Vector3 _originalPosition;
        private Transform _projectileLaunch;
        private bool _canSpawnPickUp;
        private List<GameObject> _pickUps; 

        public float UpDownSpeed;
        public float UpDownTime;

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player" && !_collisionOccured)
            {
                var collisionTotal = coll.contacts.Count();
                foreach (var contact in coll.contacts)
                {
                    Vector3 hitpos = _hitCheck.InverseTransformPoint(contact.point);
                    if (hitpos.x < 0)
                        collisionTotal--;
                }

                if (collisionTotal / coll.contacts.Count() < 1)
                {
                    //Debug.Log("Collided From Bottom");
                    _collisionOccured = true;
                    if (_canSpawnPickUp)
                    {
                        
                        _canSpawnPickUp = GameHandler.Game.Spawn.CreatePickUp(_projectileLaunch);
                        _anim.SetBool("End", !_canSpawnPickUp);
                    }
                    _timeCounter = 0f;
                }
            }
        }

        public void Start()
        {
            _anim = GetComponent<Animator>();
            _originalPosition = transform.position;
            _collisionOccured = false;
            _hitCheck = transform.FindChild("HitCheck");
            _projectileLaunch = transform.FindChild("ProjectileLaunch");
            _pickUps = GameHandler.Game.Generation.PickUps;
            _canSpawnPickUp = true;
        }

        public override void FixedUpdate()
        {
            _anim.SetBool("End", !_canSpawnPickUp);
            if (_collisionOccured && _timeCounter < UpDownTime )
            {
                transform.position = transform.position +  transform.up*Mathf.Sin(Time.time * UpDownSpeed) * Time.deltaTime;
            }
            else
            {
                _collisionOccured = false;
                transform.position = _originalPosition;
                
            }
            _timeCounter += Time.deltaTime;
        }

        

        public override void Update()
        {

        }
    }
}