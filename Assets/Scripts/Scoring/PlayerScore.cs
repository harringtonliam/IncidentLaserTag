using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Scoring
{

    public class PlayerScore : MonoBehaviour
    {
        int incidentsTagged = 0;
        int colleaguesTagged = 0;

        public int IncidentsTagged {  get { return incidentsTagged; } }
        public int ColleaguesTagged { get { return colleaguesTagged; } }


        public event Action scoreUpdated;

        private void Start()
        {
            if (scoreUpdated != null)
            {
                scoreUpdated();
            }
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

            if (scoreUpdated != null)
            {
                scoreUpdated();
            }
        }

    }
}


