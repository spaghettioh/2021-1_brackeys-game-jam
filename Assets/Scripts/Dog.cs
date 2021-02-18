using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Damageable;

public class Dog : MonoBehaviour
{
    Material material;
    Damageable damageable;
    
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        damageable = GetComponent<Damageable>();
        material = GetComponent<MeshRenderer>().material;
        originalColor = material.color;
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }

    public void DamageFlash()
    {
        StartCoroutine(ChangeInvincibilityFrameColor());
    }

    public IEnumerator ChangeInvincibilityFrameColor()
    {
        while (damageable.invincible)
        {
            Debug.Log("foo");
            material.color = Color.red;
            yield return new WaitForSeconds(.15f);
            material.color = Color.yellow;
            yield return new WaitForSeconds(.15f);
            material.color = originalColor;
        }
    }

}
