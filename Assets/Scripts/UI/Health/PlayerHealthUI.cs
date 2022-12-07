using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using TMPro;

namespace RPG.UI.HealthUI
{


    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI healthPoints;


        Health playerHealth;

        // Start is called before the first frame update
        void Awake()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            playerHealth.healthUpdated += DisplayHealth;
        }

        private void DisplayHealth()
        {
            healthPoints.text = playerHealth.HealthPoints.ToString();
        }
    }

}


