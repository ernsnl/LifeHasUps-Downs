using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Misc.TextSlide
{
    public class FloatingText : MonoBehaviour
    {
        public Animator Animator;

        public void SetText(string text)
        {
            if (Animator != null)
                Animator.GetComponent<Text>().text = text;
        }

        public void SetOpacity(float amount)
        {
            if (Animator != null)
                Animator.GetComponent<Text>().color = new Color(255, 255, 255, amount);
        }
    }
}