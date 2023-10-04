using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.GameTime;
using TMPro;

namespace RPG.UI
{
    public class GameTimeUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI gameTimeText;


        GameTimeContoller gameTimeContoller;

        // Start is called before the first frame update
        void Start()
        {
            gameTimeContoller = FindObjectOfType<GameTimeContoller>();
            gameTimeContoller.hourHasPassed += DisplayGameTime;
            DisplayGameTime();
        }

        private void DisplayGameTime()
        {
            gameTimeText.text = gameTimeContoller.CurrentHour.ToString("00") + ":00";
        }
    }
}


