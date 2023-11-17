using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Control;
using UnityEngine.Events;
using System;
using System.Collections;

namespace RPG.Attributes
{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;
        [SerializeField] float maxHealthPoints = 10f;
        [Tooltip("Time in Minutes") ]
        [SerializeField] float resuscitateTime = Mathf.Infinity;
        [SerializeField] float deathColiderHeightProportion = 0.5f;

        public event Action healthUpdated;
        ColliderController colliderController;


        bool isDead = false;
        float healthPoints;

        public bool IsDead { get { return isDead; } }

        public float HealthPoints {
            get { return healthPoints; }
        }

        void Start()
        {
            healthPoints = maxHealthPoints;
            if (healthUpdated != null)
            {
                healthUpdated();
            }
            colliderController = GetComponent<ColliderController>();
        }


        public float GetPercentage()
        {
            return ( healthPoints / GetMaxHealthPoints()) * 100;
        }

        public float GetMaxHealthPoints()
        {

            return maxHealthPoints;

        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthUpdated != null)
            {
                healthUpdated();
            }

            if (healthPoints <= 0)
            {
                Die();
            }
            else 
            {
                takeDamage.Invoke(damage);
            }
        }



        public void Heal(float healing)
        {
            healthPoints = Mathf.Min(healthPoints + healing, GetMaxHealthPoints());
            if (healthUpdated != null)
            {
                healthUpdated();
            }
        }

        public void Die()
        {
            if (isDead) return;
            StandUpFromChairIfNeeded();
            isDead = true;
            onDie.Invoke();
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("die");
                ResizeCollider();

            }

            ActionScheduler actionScheduler = GetComponent<ActionScheduler>();
            if (actionScheduler != null)
            {
                actionScheduler.CancelCurrentAction();
            }
        }

        private void ResizeCollider()
        {
            if (colliderController != null)
            {
                colliderController.ResizeCollider(deathColiderHeightProportion);
            }
        }

        private void StandUpFromChairIfNeeded()
        {
            ChairController chairController = GetComponent<ChairController>();
            if(chairController== null) return;
            if(chairController.IsSeated)
            {
                chairController.StandUpFromChair();
            }
        }

        public void Resuscitate()
        {
            StartCoroutine(UndoDie());
        }

        public void DestroyOnDeath()
        {
            Destroy(gameObject, 1f);
        }

        private IEnumerator UndoDie()
        {
            yield return new WaitForSeconds(resuscitateTime*60f);

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.ResetTrigger("stand");
                animator.SetTrigger("resuscitate");
                ResetCollider();

            }
            yield return new WaitForSeconds(resuscitateTime);
            isDead = false;


        }

        private void ResetCollider()
        {
            if (colliderController != null)
            {
                colliderController.ResetCollider();
            }
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                onDie.Invoke();
                Die();
            }
        }

        
    }
}
