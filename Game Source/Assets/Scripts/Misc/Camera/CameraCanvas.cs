using Assets.Main;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Misc.Camera
{
    public class CameraCanvas : MonoBehaviour
    {
        public Canvas CamareCanvas;

        public Texture2D cursorTexture;
        public CursorMode CursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;
        public int CursorX = 16;
        public int CursorY = 16;


        public void Awake()
        {
            Cursor.visible = false;
            Instantiate(CamareCanvas);
        }

        public void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;

                var currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                var currentResumeGame = currentCanvas.transform.FindChild("ResumePanel");

                if (currentResumeGame)
                {
                    currentResumeGame.gameObject.SetActive(true);
                    var scoreUi = currentResumeGame.FindChild("Score");
                    var seedUi = currentResumeGame.FindChild("Seed");
                    scoreUi.FindChild("ScoreValue").GetComponent<Text>().text =
                        GameHandler.Game.Player.GetComponent<SimpleScore>().GetScore().ToString();
                    var seedString = GameHandler.Game.Random.GetSeedString().Replace("-", "").ToUpper();
                    for (int i = 4; i <= seedString.Length; i += 4)
                    {
                        seedString = seedString.Insert(i, "-");
                        i++;
                    }
                    seedString = seedString.Trim('-');
                    seedUi.FindChild("SeedValue").GetComponent<Text>().text = seedString;
                }
            }
        }

        public void OnGUI()
        {
            GUI.DrawTexture(new Rect(Event.current.mousePosition.x - CursorX / 2,
                Event.current.mousePosition.y - CursorY / 2, CursorX, CursorY), cursorTexture);
        }
    }
}