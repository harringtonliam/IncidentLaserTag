using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.UI

{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] GameObject gameOverUICanvas;

        // Start is called before the first frame update
        void Start()
        {
            gameOverUICanvas.SetActive(false);
        }



        public void ShowGameOver()
        {
            Time.timeScale = 0;

            gameOverUICanvas.SetActive(true);
        }
    }



}


