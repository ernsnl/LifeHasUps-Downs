using System.Collections.Generic;
using Assets.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu_Scripts
{
    public class GameOverScreenButtons : MonoBehaviour
    {

        public void RestartLevel()
        {
            Time.timeScale = 1;

            DestroyImmediate(GameHandler.Game.Player);
            GameHandler.Game.DestroyUnnecessary();
            GameHandler.Game.Begin = true;
            GameHandler.Game.CurrentLevel = 1;
            GameHandler.Game.MinimumLevelSize = 4;
            GameHandler.Game.Random.ResetSeed();
            GameHandler.Game.SpawnNewWorld();

            var currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
            var currentGameOver = currentCanvas.transform.FindChild("GameOverPanel");
            var currentEndGame = currentCanvas.transform.FindChild("EndGamePanel");
            var currentResumePanel = currentCanvas.transform.FindChild("ResumePanel");
            currentGameOver.gameObject.SetActive(false);
            currentEndGame.gameObject.SetActive(false);
            currentResumePanel.gameObject.SetActive(false);
        }

        public void ResumeLevel()
        {
            Time.timeScale = 1;

            var currentCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
            var currentGameOver = currentCanvas.transform.FindChild("GameOverPanel");
            var currentEndGame = currentCanvas.transform.FindChild("EndGamePanel");
            var currentResumePanel = currentCanvas.transform.FindChild("ResumePanel");
            currentGameOver.gameObject.SetActive(false);
            currentEndGame.gameObject.SetActive(false);
            currentResumePanel.gameObject.SetActive(false);
        }

        public void NewGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }

        public void Exit()
        {
            Time.timeScale = 1;
            Application.Quit();
        }
    }
}