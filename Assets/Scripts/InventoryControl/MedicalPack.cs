using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Core;

namespace RPG.InventoryControl
{
    [CreateAssetMenu(menuName = ("Items/MedicalPack"))]
    public class MedicalPack : ActionItem
    {
        [SerializeField] int medicalPackHealing = 8;
        [SerializeField] int medicalPackHealingAdditiveBonus = 0;
        [SerializeField] GameObject useFX = null;


        public int MedicalPackHealing
        {
            get { return medicalPackHealing; }
        }


        public int MedicalPackHealingAdditiveBonus
        {
            get { return medicalPackHealingAdditiveBonus; }
        }

        public override void Use(GameObject user)
        {
            Health health = user.GetComponent<Health>();
            if (health == null) return;

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            if (actionScheduler != null)
            {
                actionScheduler.CancelCurrentAction();
            }



            int healingAmount = medicalPackHealing;

            health.Heal(healingAmount);
            PlayFx();


        }



        private void PlayFx()
        {
            if (useFX == null) return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject fx = GameObject.Instantiate(useFX, player.transform);
            Destroy(fx, 1f);

        }

    }
}


