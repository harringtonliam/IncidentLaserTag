using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PauseGame : MonoBehaviour
    {
        private bool isPaused = false;


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                PauseScene();
            }
            else
            {
                Time.timeScale = 1f;
            }
        }


        void PauseScene()
        {
            Time.timeScale = 0f;
        }

    }


}


