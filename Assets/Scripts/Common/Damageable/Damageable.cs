// TODO: Documentation and README
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Damageable
{
    [RequireComponent(typeof(Collider))]
    public class Damageable : MonoBehaviour
    {
        [Header("Health")]
        public int maxHitpoints = 3;
        [SerializeField]
        private int hitpoints = 3;
        public Collider hitbox;
        public UnityEvent OnHealthChange;

        [Header("Invincibility")]
        public bool invincibleAfterHit = true;
        public float invincibleSeconds = 3;
        public bool invincible;
        public UnityEvent OnDamageTaken;
        public UnityEvent OnHitpointsAtZero;
        // enum for disable or destory on death? 
    
        public void TakeDamage(int amount)
        {
            if (!invincible)
            {
                hitpoints -= amount;
                OnDamageTaken.Invoke();

                if (invincibleAfterHit)
                {
                    StartCoroutine(BecomeInvincible());
                }

                OnHealthChange.Invoke();

                // Object is "dead".
                if (hitpoints <= 0)
                {
                    OnHitpointsAtZero.Invoke();
                }
            }
        }

        public void GainHealth(int amount)
        {
            hitpoints += amount;

            // Don't go above the max
            if (hitpoints > maxHitpoints)
            {
                hitpoints = maxHitpoints;
            }

            OnHealthChange.Invoke();
        }

        IEnumerator BecomeInvincible()
        {
            invincible = true;
            yield return new WaitForSeconds(invincibleSeconds);
            invincible = false;
        }
    }

}
