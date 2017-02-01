using System.Xml.Serialization;
using Assets.Scripts.Misc.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.States
{
    public interface IEnemyState
    {
        void UpdateState();
        
        void OnTriggerEnter2D(Collider2D other);
        void OnTriggerStay2D(Collider2D other);
        void OnTriggerExit2D(Collider2D other);

        void OnCollisionEnter2D(Collision2D other);
        void OnCollisionStay2D(Collision2D other);
        void OnCollisionExit2D(Collision2D other);

        void ThrowTrigger(GlobalEnums trigg, bool enterExit);
    }
}