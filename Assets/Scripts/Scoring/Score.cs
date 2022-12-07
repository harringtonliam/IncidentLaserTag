using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Scoring
{
    public class Score : MonoBehaviour
    {
        [SerializeField] int points = 1;
        [SerializeField] ScoreType scoreType;

       

        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ScorePoints()
        {
            PlayerScore playerScore = player.GetComponent<PlayerScore>();
            playerScore.AddToScore(points, scoreType);

        }
    }

}



