using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Saving;

namespace RPG.Scoring
{

    public class PlayerScore : MonoBehaviour, ISaveable
    {
        [SerializeField] ScoreType negativeScoreType = ScoreType.Colleague;

        CurrentScores[] currentScores;


        public CurrentScores[] CurrentPlayerScores {  get { return currentScores; } }



        public event Action scoreUpdated;

        public struct CurrentScores
        {
            public ScoreType scoreType;
            public int score;
            public int maxScore;
            public bool isNegativeScore;
        }

        private void Start()
        {
            InitializeScores();
            InvokeScoreUpdated();
        }

        private void InitializeScores()
        {
            currentScores = new CurrentScores[6];
            int i = 0;
            foreach (ScoreType scoretype in Enum.GetValues(typeof(ScoreType)))
            {
                currentScores[i].scoreType = scoretype;
                currentScores[i].score = 0;
                currentScores[i].maxScore = 0;
                currentScores[i].isNegativeScore = scoretype==negativeScoreType;
                i++;
            }

            InitializeMaxScores();
        }

        public float GetPercentageScore()
        {
            float maxScore = 0;
            float currentTotalScore = 0;
            for (int i = 0; i < currentScores.Length; i++)
            {
                if (currentScores[i].scoreType != negativeScoreType)
                {
                    maxScore += currentScores[i].maxScore;
                    currentTotalScore += currentScores[i].score;
                }
                else
                {
                    currentTotalScore -= currentScores[i].score;
                }

            }
            float percenatgeScore = (currentTotalScore / maxScore) * 100;
            Debug.Log("Percentage score = " +percenatgeScore.ToString());

            return percenatgeScore;
        }

        public void InitializeMaxScores()
        {
            Score[] allScores = FindObjectsOfType<Score>();
            foreach (var score in allScores)
            {
                AddToMaxScore(score.ScoreType, score.Points);
                
            }
        }

        private void AddToMaxScore(ScoreType scoreType, int points)
        {
            for (int i = 0; i < currentScores.Length; i++)
            {
                if (currentScores[i].scoreType == scoreType)
                {
                    currentScores[i].maxScore+= points;
                }
            }
        }

        public void AddToScore(int points, ScoreType scoreType)
        {
            for (int i = 0; i < currentScores.Length; i++)
            {
                if (currentScores[i].scoreType == scoreType)
                {
                    currentScores[i].score += points;
                }
            }

            InvokeScoreUpdated();
        }


        private void InvokeScoreUpdated()
        {
            if (scoreUpdated != null)
            {
                scoreUpdated();
            }
        }

        [System.Serializable]
        private struct SavedScores
        {
            public string scoreType;
            public int score;
            public int maxScore;
        }

        public object CaptureState()
        {
            SavedScores[] savedScores = new SavedScores[currentScores.Length];
            for (int i = 0; i < currentScores.Length; i++)
            {
                savedScores[i].scoreType = currentScores[i].scoreType.ToString();
                savedScores[i].score = currentScores[i].score;
                savedScores[i].maxScore = currentScores[i].maxScore;
            }
            return savedScores;
        }

        public void RestoreState(object state)
        {
            //var savedScores = (SavedScores[])state;
            //for (int i = 0; i < savedScores.length; i++)
            //{

            //}
                
            //InvokeScoreUpdated();


        }
    }
}


