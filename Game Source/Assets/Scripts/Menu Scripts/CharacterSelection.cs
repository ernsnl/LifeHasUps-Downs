using UnityEngine;

namespace Assets.Scripts.Menu_Scripts
{
    public class CharacterSelection : MonoBehaviour
    {
        public Sprite BackGroundImage;
        private Canvas _canvas;

        public void Start()
        {
            _canvas = GetComponent<Canvas>();
            if (_canvas != null)
            {
                float width = _canvas.pixelRect.width;
                float height = _canvas.pixelRect.height;

                var charTitle = _canvas.transform.FindChild("CharacterTitle");
                var charTitleRect = charTitle.GetComponent<RectTransform>();

                var seedPanel = _canvas.transform.FindChild("SeedPanel");
                var seedPanelRect = seedPanel.GetComponent<RectTransform>();

                var navButtons = _canvas.transform.FindChild("NavigationButtons");
                var navButtonsRect = navButtons.GetComponent<RectTransform>();

                var charHover = _canvas.transform.FindChild("Character_Hover");
                var charHoverRect = charHover.GetComponent<RectTransform>();

                var charButtons = _canvas.transform.FindChild("Character Panel");
                var charButtonsRect = charButtons.GetComponent<RectTransform>();

                var totalEmptySpace = _canvas.pixelRect.width - seedPanelRect.rect.width - charButtonsRect.rect.width - charHoverRect.rect.width;
                var totalEmptySpaceY = _canvas.pixelRect.height - charHoverRect.rect.height - navButtonsRect.rect.height -
                                       charTitleRect.rect.height;

                seedPanelRect.anchoredPosition = new Vector2(-(seedPanelRect.rect.width / 2 + totalEmptySpace / 4), 0);
                charButtonsRect.anchoredPosition = new Vector2(charButtonsRect.rect.width / 2 + totalEmptySpace / 4, 0);
                navButtonsRect.anchoredPosition = new Vector2(0,navButtonsRect.rect.height / 2 + totalEmptySpaceY /4);
                charTitleRect.anchoredPosition = new Vector2(0, -charTitleRect.rect.height / 2 - totalEmptySpaceY / 4);

                var backGround = _canvas.transform.FindChild("BackgroundPanel");

                var findExit = GameObject.Find("Exit");
                for (int i = 0; i < width*2; i += 100)
                {
                    for (int j = 0; j < height*2; j += 100)
                    {
                        GameObject panel = new GameObject("BackgroundImage");
                        panel.AddComponent<CanvasRenderer>();
                        UnityEngine.UI.Image img = panel.AddComponent<UnityEngine.UI.Image>();
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
            }
        }
    }
}