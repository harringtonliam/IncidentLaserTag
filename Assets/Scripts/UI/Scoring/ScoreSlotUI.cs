using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Scoring;
using TMPro;

namespace RPG.UI.Scoring
{

    public class ScoreSlotUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreTypeText;
        [SerializeField] TextMeshProUGUI currentScoretext;
        [SerializeField] RectTransform foregroundProgressBar = null;
        [SerializeField] GameObject progressBar = null;

        float currentScore;
        float maxPossibleScore;
        bool isNegativeScore = false;

        public void Setup(ScoreType scoreType, int score, int maxScore, bool negativeScore)
        {
            this.currentScore = score;
            this.maxPossibleScore = maxScore;
            isNegativeScore = negativeScore;

            scoreTypeText.text = scoreType.ToString();
            UpdateScore(currentScore, maxPossibleScore);

            if (isNegativeScore)
            {
                progressBar.SetActive(false);
            }
        }

        public void UpdateScore( float score, float maxScore)
        {
            if(isNegativeScore)
            {
                currentScoretext.text = "minus " + score.ToString();
            }
            else
            {
                currentScoretext.text = score.ToString() + "/" + maxScore.ToString();
                DisplayProgressBar();
            }


        }

        private void DisplayProgressBar()
        {
            if (foregroundProgressBar == null) return;

            if (maxPossibleScore == 0 )
            {
                foregroundProgressBar.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                float scoreFraction = Mathf.Clamp(currentScore / maxPossibleScore, 0f, 1f);
                Vector3 newScale = new Vector3(scoreFraction , 1f, 1f);
                foregroundProgressBar.localScale = newScale;
            }

        }

    }
}


