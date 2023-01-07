using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Scoring
{

    [CreateAssetMenu(menuName = ("Score/Score Range"))]
    public class ScoreRanges : ScriptableObject
    {
        [SerializeField] ScoreMessage[] scoreMessages;

        [Serializable]
        public struct ScoreMessage
        {
            public float percentage;
            public string message;
        }

        public string GetPlayerScoreMessage()
        {
            PlayerScore playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScore>();
            float playerScorePercentage = playerScore.GetPercentageScore();

            for (int i = 0; i < scoreMessages.Length; i++)
            {
                if (playerScorePercentage <= scoreMessages[i].percentage)
                {
                    return scoreMessages[i].message;
                }
            }

            return scoreMessages[scoreMessages.Length - 1].message;

        }

    }
}


