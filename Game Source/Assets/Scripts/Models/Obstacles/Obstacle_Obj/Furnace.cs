using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Models.Obstacles.Obstacle_Obj
{
    public class Furnace : Obstacle
    {
        public int heartLost;

        public override void Update()
        {
            
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                var playerHealth = coll.gameObject.GetComponent<SimpleHealth>();
                playerHealth.LoseHealth(heartLost, transform);
            }
        }

        public void OnCollisionStay2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                var playerHealth = coll.gameObject.GetComponent<SimpleHealth>();
                playerHealth.LoseHealth(heartLost, transform);
            }
        }
    }
}