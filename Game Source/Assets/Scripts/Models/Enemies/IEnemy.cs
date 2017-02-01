using Assets.Scripts.Misc.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies
{
    public interface IEnemy
    {
        void Update();
        void FixedUpdate();


        //Mainly For State Transition
        void OnTriggerEnter2D(Collider2D coll);
        void OnTriggerStay2D(Collider2D coll);
        void OnTriggerExit2D(Collider2D coll);

        //Mainly For Smoother Gameplay
        void OnCollisionEnter2D(Collision2D coll);
        void OnCollisionStay2D(Collision2D coll);
        void OnCollisionExit2D(Collision2D coll);

        //Mainly For Avoiding Unexpected Behaiour
        void ThrowTrigger(GlobalEnums trigger, bool enterExit);

        void OnDestroy();
    }
}