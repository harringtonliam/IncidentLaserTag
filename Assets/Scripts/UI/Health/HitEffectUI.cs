using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.HealthUI

{
    public class HitEffectUI : MonoBehaviour
    {
        [SerializeField] GameObject hitEffect;
        [SerializeField] float showHitEffectForSeconds =  0.5f;

        // Start is called before the first frame update
        void Start()
        {

            hitEffect.SetActive(false);
        }



        public void HandleHitEffect()
        {
            StartCoroutine(ShowHitEffect());
        }

        private IEnumerator ShowHitEffect()
        {
            hitEffect.SetActive(true);

            yield return new WaitForSeconds(showHitEffectForSeconds);

            hitEffect.SetActive(false);

        }
    }
}


