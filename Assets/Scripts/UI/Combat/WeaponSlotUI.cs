using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RPG.Combat;

namespace RPG.UI.Combat
{
    public class WeaponSlotUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI weaponLabel;
        [SerializeField] TextMeshProUGUI ammunition;
        [SerializeField] Image weaponImage;
        [SerializeField] GameObject activeWeaponImage;
        [SerializeField] string outOfAmmunitionText = "Out!";


        AmmunitionType ammunitionType;

        public AmmunitionType GetAmmunitionType() { return ammunitionType; }

        public void Setup(Sprite weaponIcon, string weaponDesc, int ammunitionAmount, bool activeWeapon, AmmunitionType ammunitionType)
        {
            weaponLabel.text = weaponDesc + ": ";
            UpdateAmmunitionAmount(ammunitionAmount);
            weaponImage.sprite = weaponIcon;
            activeWeaponImage.SetActive(activeWeapon);
            this.ammunitionType = ammunitionType;
        }    

        public void UpdateAmmunitionAmount(int ammunitionAmount)
        {
            if (ammunitionAmount > 0)
            {
                ammunition.text = ammunitionAmount.ToString();
            }
            else
            {
                ammunition.text = outOfAmmunitionText;
            }
            
        }
    }
}


