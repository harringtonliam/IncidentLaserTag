using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{

    public class PauseGame : MonoBehaviour
    {


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)   )
            {
                if (Mathf.Approximately(Time.timeScale , 0f))
                {
                    Time.timeScale = 1f;
                }
                else
                {
                    Time.timeScale = 0f;
                }
      
            }
        }
    }
}



