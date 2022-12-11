using UnityEngine;
using RPG.Saving;
using RPG.Core;
using UnityEngine.Events;
using System;

namespace RPG.Attributes
{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;
        [SerializeField] float maxHealthPoints = 10f;

        public event Action healthUpdated;


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
            onDie.Invoke();
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("die");
            }

            ResizeCapsuleColliderOnDeath();

            isDead = true;

            ActionScheduler actionScheduler = GetComponent<ActionScheduler>();
            if (actionScheduler != null)
            {
                actionScheduler.CancelCurrentAction();
            }
        }

        private void ResizeCapsuleColliderOnDeath()
        {
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                capsuleCollider.height = capsuleCollider.height / 10f;
                capsuleCollider.center = capsuleCollider.center / 4f;
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
