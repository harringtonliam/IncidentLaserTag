using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Scoring
{
    public class Score : MonoBehaviour
    {
        [SerializeField] int points = 1;
        [SerializeField] ScoreType scoreType;
       

        public ScoreType ScoreType {  get { return scoreType; } }
        public int Points { get { return points; } }

        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }



        public void ScorePoints()
        {
            PlayerScore playerScore = player.GetComponent<PlayerScore>();
            playerScore.AddToScore(points, scoreType);

        }
    }

}



