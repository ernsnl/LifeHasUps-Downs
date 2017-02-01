using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu_Scripts
{
    public class MenuImageBackground : MonoBehaviour
    {
        public Sprite BackGroundImage;
        public Sprite RopeImage;
        public Sprite RopeEnd;

        private Canvas _canvas;
             
        public void Awake()
        {
            Cursor.visible = true;
            _canvas = GetComponent<Canvas>();
            if (_canvas != null)
            {
                float width = _canvas.pixelRect.width;
                float height = _canvas.pixelRect.height;

                var buttonPanel = _canvas.transform.FindChild("Panel");
                var buttonPanelRect = buttonPanel.GetComponent<RectTransform>();

                var titleImage = _canvas.transform.FindChild("TitleImage");                
                var titleRect = titleImage.GetComponent<RectTransform>();

                var rope = _canvas.transform.FindChild("Rope");
                var ropeRect = rope.GetComponent<RectTransform>();


                var totalEmptySpace = _canvas.pixelRect.width - titleRect.rect.width - buttonPanelRect.rect.width;

                titleRect.anchoredPosition = new Vector2(-(titleRect.rect.width/2 + totalEmptySpace / 4), 0);
                buttonPanelRect.anchoredPosition = new Vector2(buttonPanelRect.rect.width/2 + totalEmptySpace /4, 0);
                ropeRect.anchoredPosition = new Vector2(ropeRect.rect.width / 2 - buttonPanelRect.rect.width / 20 + totalEmptySpace / 4, 0);

                var backGround = _canvas.transform.FindChild("BackgroundPanel");
                           
                var findExit = GameObject.Find("Exit");
                for (int i = 0; i < width * 2; i += 100)
                {
                    for (int j = 0; j < height * 2; j += 100)
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
                    for (int j = 0; j < height*85/100; j += 30)
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
    }
}