using UnityEngine;
using UnityEngine.Events;

namespace Common.Damageable
{
    [RequireComponent(typeof(Collider))]
    public class Damager : MonoBehaviour
    {
        [Header("Damage")]
        public int attackPoints = 1;
        public LayerMask hitableLayers;
        public bool ignoresInvincibility;
        public bool damagesTriggers;

        [Header("After damage dealt")]
        [Tooltip("Perform when this object collides with a Damageable object.")]
        public UnityEvent OnDamageableHit;
        [Tooltip("Perform when this object collides with a non-Damageable object.")]
        public UnityEvent OnNonDamageableHit;

        private void OnCollisionEnter(Collision c)
        {
            if (CheckForHitbox(c.collider))
            {
                DealDamage(c.gameObject);
            }
            else
            {
                OnNonDamageableHit.Invoke();
            }
        }

        private void OnTriggerEnter(Collider c)
        {
            if (CheckForHitbox(c) && damagesTriggers)
            {
                DealDamage(c.gameObject);
            }
            else
            {
                OnNonDamageableHit.Invoke();
            }
        }

        private bool CheckForHitbox(Collider c)
        {
            if (hitableLayers == (hitableLayers | (1 << c.gameObject.layer)))
            {
                if (c.gameObject.GetComponent<Damageable>())
                {
                    Damageable d = c.gameObject.GetComponent<Damageable>();
                    if (c == d.hitbox)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void DealDamage(GameObject g)
        {
            Damageable damageable = g.GetComponent<Damageable>();

            if (damageable != null)
            {
                // TODO: Add check for hitable layer match.
                damageable.TakeDamage(attackPoints);
                if (!damageable.invincible || ignoresInvincibility)
                {
                    OnDamageableHit.Invoke();
                }
            }
            else
            {
                OnNonDamageableHit.Invoke();
            }
        }
    }

}
