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
        [SerializeField] RectTransform foregroundHealthBar = null;


        Health playerHealth;


        // Start is called before the first frame update
        void Awake()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            playerHealth.healthUpdated += DisplayHealth;
        }



        private void DisplayHealth()
        {
            DisplayHealthText();
            DisplayHealthBar();
        }

        private void DisplayHealthBar()
        {
            if (foregroundHealthBar == null) return;
            Vector3 newScale = new Vector3(playerHealth.HealthPoints / playerHealth.GetMaxHealthPoints(), 1, 1);
            foregroundHealthBar.localScale = newScale;
        }

        private void DisplayHealthText()
        {
            healthPoints.text = playerHealth.HealthPoints.ToString();
            SetHelthPointTextColor();
        }

        private void SetHelthPointTextColor()
        {
            if (playerHealth.HealthPoints < (playerHealth.GetMaxHealthPoints() * 0.33f))
            {
                healthPoints.color = Color.red;
            }
            else if (playerHealth.HealthPoints < (playerHealth.GetMaxHealthPoints() * 0.66f))
            {
                healthPoints.color = Color.yellow;
            }
            else if (playerHealth.HealthPoints < (playerHealth.GetMaxHealthPoints()))
            {
                healthPoints.color = Color.green;
            }
            else
            {
                healthPoints.color = Color.white;
            }
        }
    }

}


