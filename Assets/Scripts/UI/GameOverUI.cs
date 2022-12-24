using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;


namespace RPG.UI

{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] GameObject gameOverUICanvas;
        [SerializeField] SceneController sceneController;

        // Start is called before the first frame update
        void Start()
        {
            gameOverUICanvas.SetActive(false);
        }



        public void ShowGameOver()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverUICanvas.SetActive(true);
        }

        public void PlayAgainButtonClick()
        {
            sceneController.LoadGame();
        }

        public void MainMenuButtonClick()
        {
            sceneController.LoadMainMenu();
        }
    }



}


