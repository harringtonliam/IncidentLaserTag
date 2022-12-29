using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Scoring;
using TMPro;

namespace RPG.UI.Scoring
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] ScoreSlotUI scoreSlotUIPrefab;
        [SerializeField] TextMeshProUGUI totalScoreText;

        PlayerScore playerScore;

        // Start is called before the first frame update
        void Awake()
        {
                playerScore = GameObject.FindWithTag("Player").GetComponent<PlayerScore>();
                playerScore.scoreUpdated += DisplayScore;
        }


        private void DisplayScore()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            int totalScore = 0;
            foreach (var item in playerScore.CurrentPlayerScores)
            {
                totalScore = UpdateTotalScore(totalScore, item);

                ScoreSlotUI scoreSlotUI = Instantiate(scoreSlotUIPrefab, transform);
                scoreSlotUI.Setup(item.scoreType, item.score, item.maxScore, item.isNegativeScore);
            }

            totalScoreText.text = totalScore.ToString();

        }

        private  int UpdateTotalScore(int totalScore, PlayerScore.CurrentScores item)
        {
            if (item.isNegativeScore)
            {
                totalScore = totalScore - item.score;
            }
            else
            {
                totalScore = totalScore + item.score;
            }

            return totalScore;
        }
    }

}


