using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Saving;

namespace RPG.Scoring
{

    public class PlayerScore : MonoBehaviour, ISaveable
    {
        int incidentsTagged = 0;
        int colleaguesTagged = 0;

        public int IncidentsTagged {  get { return incidentsTagged; } }
        public int ColleaguesTagged { get { return colleaguesTagged; } }


        public event Action scoreUpdated;

        private void Start()
        {
            InvokeScoreUpdated();
        }

        public void AddToScore(int points, ScoreType scoreType)
        {
            if (scoreType == ScoreType.Colleague)
            {
                colleaguesTagged = colleaguesTagged + points;
            }
            if (scoreType == ScoreType.Incident)
            {
                incidentsTagged = incidentsTagged + points;
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
            public int savedIncidentsTagged;
            public int savedColleaguesTagged;
        }

        public object CaptureState()
        {
            SavedScores savedScores = new SavedScores();
            savedScores.savedIncidentsTagged = incidentsTagged;
            savedScores.savedColleaguesTagged = colleaguesTagged;
            return savedScores;
        }

        public void RestoreState(object state)
        {
            SavedScores savedScores = (SavedScores)state;
            incidentsTagged = savedScores.savedIncidentsTagged;
            colleaguesTagged = savedScores.savedColleaguesTagged;
            InvokeScoreUpdated();


        }
    }
}


