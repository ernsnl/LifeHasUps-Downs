using Assets.Main;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu_Scripts
{
    public class GameOverScreen : MonoBehaviour
    {
        public Sprite BackGroundImage;
        public Sprite RopeImage;
        public Sprite RopeEnd;

        private Canvas _canvas;

        private void BackGroundHandle(Transform parent)
        {
            if (parent != null)
            {
                var backGround = parent.transform.FindChild("GameOverBackground");
                var rope = parent.transform.FindChild("Rope");
                var findExit = GameObject.Find("Exit");
                float width = _canvas.pixelRect.width;
                float height = _canvas.pixelRect.height;
                for (int i = 0; i < width * 1.25f; i += 100)
                {
                    for (int j = 0; j < height * 1.25f; j += 100)
                    {
                        GameObject panel = new GameObject("BackgroundImage");
                        panel.AddComponent<CanvasRenderer>();
                        Image img = panel.AddComponent<Image>();
                        img.sprite = BackGroundImage;
                        var panelRect = panel.GetComponent<RectTransform>();
                        panelRect.anchorMax = new Vector2(0, 1);
                        panelRect.anchorMin = new Vector2(0, 1);
                        panelRect.anchoredPosition = new Vector2(i, -j);
                        panelRect.sizeDelta = new Vector2(100, 100);
                        panelRect.localPosition = new Vector3(panelRect.localPosition.x, panelRect.localPosition.y, -1f);
                        panel.transform.SetParent(backGround.transform, false);
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    float lastPosition = 0;
                    for (int j = 0; j < height * 85 / 100; j += 30)
                    {
                        GameObject panel = new GameObject("RopePart");
                        panel.AddComponent<CanvasRenderer>();
                        Image img = panel.AddComponent<Image>();
                        img.sprite = RopeImage;
                        var panelRect = panel.GetComponent<RectTransform>();
                        panelRect.anchorMax = new Vector2(0, 1);
                        panelRect.anchorMin = new Vector2(0, 1);
                        panelRect.anchoredPosition = new Vector2(75 + i * 175, -j);
                        panelRect.sizeDelta = new Vector2(22, 30);
                        panelRect.localPosition = new Vector3(panelRect.localPosition.x, panelRect.localPosition.y, -1f);
                        panel.transform.SetParent(rope.transform, false);
                        lastPosition = j;
                    }
                    GameObject ropeEnd = new GameObject("RopeEnd");
                    ropeEnd.AddComponent<CanvasRenderer>();
                    Image ropeEndImage = ropeEnd.AddComponent<Image>();
                    ropeEndImage.sprite = RopeEnd;
                    var ropeEndRect = ropeEnd.GetComponent<RectTransform>();
                    ropeEndRect.anchorMax = new Vector2(0, 1);
                    ropeEndRect.anchorMin = new Vector2(0, 1);
                    ropeEndRect.anchoredPosition = new Vector2(75 + i * 175, -lastPosition - 30);
                    ropeEndRect.sizeDelta = new Vector2(22, 30);
                    ropeEndRect.localPosition = new Vector3(ropeEndRect.localPosition.x, ropeEndRect.localPosition.y, -1f);
                    ropeEnd.transform.SetParent(rope.transform, false);
                }

            }
        }

        public void Awake()
        {
            _canvas = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<Canvas>();

            var gameOverPanel = _canvas.transform.FindChild("GameOverPanel");
            var endGamePanel = _canvas.transform.FindChild("EndGamePanel");
            var resumePanel = _canvas.transform.FindChild("ResumePanel");
                  
            BackGroundHandle(gameOverPanel);
            BackGroundHandle(endGamePanel);
            BackGroundHandle(resumePanel);

        }
    }
}