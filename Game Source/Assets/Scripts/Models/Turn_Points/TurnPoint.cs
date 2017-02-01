using Assets.Scripts.Misc.Enums;
using Assets.Scripts.Models.Enemies;
using UnityEngine;

namespace Assets.Scripts.Models.Turn_Points
{
    public class TurnPoint : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Enemy" && !coll.isTrigger)
            {
                var component = coll.gameObject.GetComponent<IEnemy>();
                component.ThrowTrigger(GlobalEnums.CurvePoints, true);
            }
        }

        public void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Enemy" && !coll.isTrigger)
            {
                var component = coll.gameObject.GetComponent<IEnemy>();
                component.ThrowTrigger(GlobalEnums.CurvePoints, false);
            }
        }

        public void OnTriggerStayt2D(Collider2D coll)
        {
            if (coll.gameObject.tag == "Enemy" && !coll.isTrigger)
            {
                var component = coll.gameObject.GetComponent<IEnemy>();
                component.ThrowTrigger(GlobalEnums.CurvePoints, true);
            }
        }
    }
}