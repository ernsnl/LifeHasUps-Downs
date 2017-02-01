using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Misc.TextSlide
{
    public class FloatingTextController : MonoBehaviour
    {
        private static FloatingText popUpText;
        private static GameObject canvas;

        public static void Initialize()
        {
            popUpText = Resources.Load<FloatingText>("Prefabs/Misc/ItemRoom_TextSlide");
            canvas = GameObject.FindGameObjectWithTag("GameCanvas");
        }

        public static FloatingText CreateFloatingText(string text, Transform location)
        {
            if (GameObject.FindGameObjectWithTag("GameCanvas"))
            {
                FloatingText instance = Instantiate(Resources.Load<FloatingText>("Prefabs/Misc/ItemRoom_TextSlide"));
                Vector2 screenPosition = UnityEngine.Camera.main.WorldToScreenPoint(location.position);
                instance.transform.SetParent(GameObject.FindGameObjectWithTag("GameCanvas").transform, false);
                instance.SetText(text);
                instance.transform.position = screenPosition;
                instance.transform.rotation = instance.transform.parent.rotation;

                return instance;
            }
            return null;
        }


        
    }
}