using UnityEngine;

namespace Assets.Scripts.Models.Obstacles
{
    public class Obstacle : MonoBehaviour, IObstacle
    {
        public float OffSet = 0f;

        public virtual void AfterAttack() { }

        public virtual void AfterExplosion() { }

        public virtual void Update() { }

        public virtual void FixedUpdate()
        {
            
        }
    }
}