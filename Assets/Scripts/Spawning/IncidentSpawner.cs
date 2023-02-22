using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.GameTime;
using RPG.Scoring;
using System;

namespace RPG.Spawning
{




    public class IncidentSpawner : MonoBehaviour
    {
        [SerializeField] IncidentSpawnDetail[] incidentSpawnDetails;
        [SerializeField] Transform spawnPoint;

        [Serializable]
        struct IncidentSpawnDetail
        {
            public GameObject incidentPrefab;
            public int percentChance;
            public PatrolPath patrolPath;
        }

        public event Action incidentSpawned;


        private GameTimeContoller gameTimeController;

        // Start is called before the first frame update
        void Start()
        {
            gameTimeController = FindObjectOfType<GameTimeContoller>();
            gameTimeController.hourHasPassed += SpawningProcess;
        }


        private void SpawningProcess()
        {


            int randomwNumber = UnityEngine.Random.Range(1, 100);

            foreach (var item in incidentSpawnDetails)
            {
                if (randomwNumber <= item.percentChance)
                {
                    SpawnIncident(item);
                    break;
                }
            }

        }

        private void SpawnIncident(IncidentSpawnDetail incidentSpawnDetail)
        {
            GameObject newIncident = GameObject.Instantiate(incidentSpawnDetail.incidentPrefab, spawnPoint.position, Quaternion.identity);
            newIncident.transform.parent = this.transform;

            SetNewIncidentPatrolPath(incidentSpawnDetail, newIncident);
            AddNewIncidentToMaxScore(newIncident);

            if (incidentSpawned != null)
            {
                incidentSpawned();
            }
        }

        private static void AddNewIncidentToMaxScore(GameObject newIncident)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerScore playerScore = player.GetComponent<PlayerScore>();
            playerScore.AddToMaxScore(newIncident.GetComponent<Score>().ScoreType, 1);
            playerScore.InvokeScoreUpdated();
        }

        private static void SetNewIncidentPatrolPath(IncidentSpawnDetail incidentSpawnDetail, GameObject newIncident)
        {
            AIControler newIncidentAIController = newIncident.GetComponent<AIControler>();
            if (newIncidentAIController != null)
            {
                newIncidentAIController.SetPatrolPath(incidentSpawnDetail.patrolPath, true);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1f);
            Gizmos.DrawWireSphere(spawnPoint.transform.position, 0.5f);
        }
    }

}
