using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Spawning;


namespace RPG.UI
{

    public class NewIncidentUI : MonoBehaviour
    {
        [SerializeField] IncidentSpawner incidentSpawner;
        [SerializeField] TextMeshProUGUI messageDisplayText;
        [SerializeField] GameObject newIncidentObject;
        [SerializeField] string messageText = "New Incident {0} Detected!";
        [SerializeField] float messageDisplayTime = 2f;

        // Start is called before the first frame update
        void Start()
        {
            messageDisplayText.text = "";
            newIncidentObject.SetActive(false);
            incidentSpawner.incidentSpawned += TriggerMessage;
        }


        private void TriggerMessage()
        {
            StartCoroutine(ShowMessageText());
        }

        private IEnumerator ShowMessageText()
        {
            int randomIncidentNumber = Random.Range(1000, 100000);
            string incidentNumberText = "INC" + randomIncidentNumber.ToString("0000000");
            string newMessageText = string.Format(messageText, incidentNumberText);
            newIncidentObject.SetActive(true);
            messageDisplayText.text = newMessageText;
            yield return new WaitForSeconds(messageDisplayTime);
            messageDisplayText.text = "";
            newIncidentObject.SetActive(false);
        }
    }

}


