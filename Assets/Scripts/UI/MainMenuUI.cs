using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;

namespace  RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject controlsCanavas;
        [SerializeField] RPG.SceneManagement.SceneController sceneContoller;


        private void Start()
        {
            ShowHideControlsCanvas(false);
        }


        public void ShowHideControlsCanvas(bool visible)
        {
            controlsCanavas.SetActive(visible);

        }

        public void PlayGame()
        {
            sceneContoller.LoadGame();
        }
    }

}


