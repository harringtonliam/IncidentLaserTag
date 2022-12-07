using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Scoring;
using TMPro;

namespace RPG.UI.Scoring
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI incidentsTaggedScore;
        [SerializeField] TextMeshProUGUI colleaguesTaggedScore;
        [SerializeField] TextMeshProUGUI totalScore;

        PlayerScore playerScore;

        // Start is called before the first frame update
        void Awake()
        {
                playerScore = GameObject.FindWithTag("Player").GetComponent<PlayerScore>();
                playerScore.scoreUpdated += DisplayScore;
        }


        private void DisplayScore()
        {
            incidentsTaggedScore.text = playerScore.IncidentsTagged.ToString();
            colleaguesTaggedScore.text = playerScore.ColleaguesTagged.ToString();
            totalScore.text = (playerScore.IncidentsTagged - playerScore.ColleaguesTagged).ToString();


        }

    }

}


