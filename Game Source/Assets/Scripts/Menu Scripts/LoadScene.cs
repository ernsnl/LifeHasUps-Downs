using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Assets.Main;
using Assets.Scripts.Misc.Handlers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public GameObject gameHandler;

    private string seedNumber;

    public InputField inputField;

    public void Start()
    {
        if (GameHandler.Game == null)
        {
            Instantiate(gameHandler);
        }

        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(delegate { beautifyInput(inputField.text); });
            inputField.onEndEdit.AddListener(delegate { checkSeed(inputField.text); });
        }
    }

    public void LoadMenu(int menuID)
    {
        SceneManager.LoadScene(menuID);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadLevel(int characterID)
    {
        if (inputField != null)
        {
            seedNumber = inputField.text.Trim();
            if (seedNumber.Length == 14)
            {
                //Debug.Log("Seed Is Okay");
            }
            else
            {
                seedNumber = GameHandler.Game.Random.RandomString(12);
                inputField.text = "";
                //Debug.Log("Seed Is Not Okay");
            }
        }

        GameHandler.Game.DestroyUnnecessary();
        var player = GameHandler.Game.Player;

        if (player)
            Destroy(player);

        GameHandler.Game.Random.SetSeed(seedNumber);
        PlayerPrefs.SetString("seedNumber", seedNumber);
        GameHandler.Game.Begin = true;
        SceneManager.LoadScene("Minimum Scene");
    }

    #region Seed

    public void beautifyInput(string arg0)
    {
        arg0 = arg0.ToUpper();
        string beautifyiedSeed = AlphaNumericCheck(arg0.Replace("-", ""));
        if (beautifyiedSeed.Length > 4 && beautifyiedSeed.Length <= 12)
        {
            var beautifyiedSeedSlice = beautifyiedSeed.ToCharArray();
            int counter = 1;
            beautifyiedSeed = "";
            foreach (var charValue in beautifyiedSeedSlice)
            {
                beautifyiedSeed += charValue.ToString() +
                                   (counter % 4 == 0 && beautifyiedSeedSlice.Length != counter ? "-" : "");
                counter++;
            }
            inputField.MoveTextEnd(false);
        }
        if (arg0.Length <= 14)
            inputField.text = beautifyiedSeed.ToUpper();

    }

    public string AlphaNumericCheck(string text)
    {
        text = Regex.Replace(text, @"[^a-zA-Z0-9 ]", "");
        return text;
    }

    public void checkSeed(string arg0)
    {
        if (arg0.Length != 14)
            inputField.text = "";
    }

    #endregion 
}
