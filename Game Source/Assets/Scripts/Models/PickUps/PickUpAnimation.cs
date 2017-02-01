using UnityEngine;

namespace Assets.Scripts.Models.PickUps
{
    public class PickUpAnimation : MonoBehaviour
    {

        public bool Animate = true;
        public float MoveRange = 0.004f;
        public float MoveSpeed = 5f;

        private Transform _startTransform;

        void Start()
        {
            _startTransform = this.transform;
        }

        void FixedUpdate()
        {
            var castRay = Physics2D.Raycast(transform.position, -1 * transform.up, 0.5f, 1 << LayerMask.NameToLayer("TerrainLayerMask"));


            if (Animate && castRay.collider != null)
            {
                this.transform.position = this.transform.position +
                                          transform.up*(MoveRange*Mathf.Sin(Time.timeSinceLevelLoad*MoveSpeed));
            }
        }

    }
}