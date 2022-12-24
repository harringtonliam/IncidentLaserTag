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

        int currentScore;
        int maxPossibleScore;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Setup(ScoreType scoreType, int score, int maxScore)
        {
            this.currentScore = score;
            this.maxPossibleScore = maxScore;

            scoreTypeText.text = scoreType.ToString();
            UpdateScore(currentScore, maxPossibleScore);
        }

        public void UpdateScore( int score, int maxScore)
        {
            currentScoretext.text = score.ToString() + "/" + maxScore.ToString();
            DisplayProgressBar();
        }

        private void DisplayProgressBar()
        {
            if (foregroundProgressBar == null) return;
            if (maxPossibleScore ==0 )
            {
                foregroundProgressBar.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                Vector3 newScale = new Vector3(Mathf.Clamp(currentScore, 0, 1) / maxPossibleScore, 1, 1);
                foregroundProgressBar.localScale = newScale;
            }

        }

    }
}


