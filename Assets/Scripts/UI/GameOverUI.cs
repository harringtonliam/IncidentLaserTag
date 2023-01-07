using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
using RPG.Attributes;
using RPG.Scoring;
using TMPro;


namespace RPG.UI

{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] GameObject gameOverUICanvas;
        [SerializeField] SceneController sceneController;
        [SerializeField] TextMeshProUGUI resultText;
        [SerializeField] string playerDeathMessage;
        [SerializeField] string compeleteMessage;
        [SerializeField] TextMeshProUGUI performanceAssessmentText;
        [SerializeField] ScoreRanges scoreRanges;

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

            ShowResultMessage();
            ShowPerformanceAssessment();
        }

        private void ShowResultMessage()
        {
            Health playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            if (playerHealth.IsDead)
            {
                resultText.text = playerDeathMessage;
            }
            else
            {
                resultText.text = compeleteMessage;
            }
        }

        private void ShowPerformanceAssessment()
        {
            performanceAssessmentText.text = scoreRanges.GetPlayerScoreMessage();
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


